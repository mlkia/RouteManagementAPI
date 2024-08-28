using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Entities;
using RouteManagementTutorial.Models;
using RouteManagementTutorial.Helper;
using RouteManagementTutorial.DTO;
using Route = Entities.Route; //This is to avoid confusion with the Route class in the 'Microsoft.AspNetCore.Routing.Route'

namespace RouteManagementTutorial.Services
{
    public class RouteService
    {
        private readonly IMongoCollection<Route> _routesCollection;
        public RouteService(IOptions<RouteManagementTutorialDataBaseSettings> RMTDataBasesettings)
        {
            var mongoclient = new MongoClient(RMTDataBasesettings.Value.ConnectionString);
            var database = mongoclient.GetDatabase(RMTDataBasesettings.Value.DatabaseName);

            _routesCollection = database.GetCollection<Route>(RMTDataBasesettings.Value.RoutesCollectionName);
        }

        public async Task<List<Route>> GetAsync() =>
            await _routesCollection.Find(_ => true).ToListAsync();

        public async Task<Route?> GetAsync(string id) =>
            await _routesCollection.Find(x => x.RouteId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Route newRoute) =>
            await _routesCollection.InsertOneAsync(newRoute);
    }
}
