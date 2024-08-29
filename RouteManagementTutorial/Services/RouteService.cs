using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Entities;
using RouteManagementTutorial.Models;
using RouteManagementTutorial.Helper;
using RouteManagementTutorial.DTO;
using Route = Entities.Route;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc; //This is to avoid confusion with the Route class in the 'Microsoft.AspNetCore.Routing.Route'

namespace RouteManagementTutorial.Services
{
    public class RouteService
    {
        private readonly IMongoCollection<Route> _routesCollection;
        private readonly DriversService _driversService;
        public RouteService(IOptions<RouteManagementTutorialDataBaseSettings> RMTDataBasesettings, DriversService driversService)
        {
            var mongoclient = new MongoClient(RMTDataBasesettings.Value.ConnectionString);
            var database = mongoclient.GetDatabase(RMTDataBasesettings.Value.DatabaseName);

            _routesCollection = database.GetCollection<Route>(RMTDataBasesettings.Value.RoutesCollectionName);
            _driversService = driversService;
        }

        public async Task<List<Route>> GetAsync() =>
            await _routesCollection.Find(_ => true).ToListAsync();

        public async Task<Route?> GetAsync(string id) =>
            await _routesCollection.Find(x => x.RouteId == id).FirstOrDefaultAsync();

        public async Task<CreateRouteResult> CreateAsync(Route newRoute)
        {
            var driver = await _driversService.GetAsync(newRoute.DriverID);

            if (driver is null)
            {
                return new CreateRouteResult { Success = false, Message= "Driver Not Found" }; 
            }

            newRoute.DriverName = driver.Name;

            await _routesCollection.InsertOneAsync(newRoute);

            return new CreateRouteResult { Success = true, Route = newRoute };
        }

    }

    public class CreateRouteResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Route? Route { get; set; }
    }

}
