using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RouteManagementTutorial.Models;
using RouteManagementTutorial.Entities; 


namespace RouteManagementTutorial.Services
{
    public class DriversService
    {
        private readonly IMongoCollection<Driver> _driversCollection;

        public DriversService(IOptions<RouteManagementTutorialDataBaseSettings> RMTDataBasesettings)
        {
            var mongoclient = new MongoClient(RMTDataBasesettings.Value.ConnectionString);
            var database = mongoclient.GetDatabase(RMTDataBasesettings.Value.DatabaseName);

            _driversCollection = database.GetCollection<Driver>(RMTDataBasesettings.Value.DriversCollectionName);
        }

        public async Task<List<Driver>> GetAsync() => 
            await _driversCollection.Find(_ => true).ToListAsync();

        public async Task<Driver?> GetAsync(string id) =>
            await _driversCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Driver newDriver) =>
            await _driversCollection.InsertOneAsync(newDriver);

        public async Task UpdateAsync(string id, Driver updatedDriver) =>
            await _driversCollection.ReplaceOneAsync(x => x.Id == id, updatedDriver);

        public async Task DeleteAsync(string id) =>
            await _driversCollection.DeleteOneAsync(x => x.Id == id);
        
    }
}
