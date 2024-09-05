using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RouteManagementTutorial.Models;
using RouteManagementTutorial.Entities;
using RouteManagementTutorial.Helper;
using RouteManagementTutorial.DTO;
using RouteManagementTutorial.Authenticate;


namespace RouteManagementTutorial.Services
{
    public class DriversService
    {
        private readonly IMongoCollection<Driver> _driversCollection;
        private readonly CreateAuthentication _createAuthentication;

        public DriversService(IOptions<RouteManagementTutorialDataBaseSettings> RMTDataBasesettings,CreateAuthentication createAuthentication)
        {
            var mongoclient = new MongoClient(RMTDataBasesettings.Value.ConnectionString);
            var database = mongoclient.GetDatabase(RMTDataBasesettings.Value.DatabaseName);

            _driversCollection = database.GetCollection<Driver>(RMTDataBasesettings.Value.DriversCollectionName);
            _createAuthentication = createAuthentication;
        }

        /// <summary>
        /// Retrieves all drivers from the database.
        /// </summary>
        /// <returns>A list of all drivers.</returns>
        public async Task<List<Driver>> GetAsync() => 
            await _driversCollection.Find(_ => true).ToListAsync();


        /// <summary>
        /// Retrieves a driver by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the driver.</param>
        /// <returns>The driver with the specified identifier, or <c>null</c> if no such driver exists.</returns>
        public async Task<Driver?> GetAsync(string id) =>
            await _driversCollection.Find(x => x.Id == id).FirstOrDefaultAsync();


        /// <summary>
        /// Retrieves a driver by their email address.
        /// </summary>
        /// <param name="email">The email address of the driver.</param>
        /// <returns>The driver with the specified email address, or <c>null</c> if no such driver exists.</returns>
        public async Task<Driver?> GetByEmail(string email) =>
            await _driversCollection.Find(x => x.Email == email).FirstOrDefaultAsync();


        /// <summary>
        /// Creates a new driver and validates their email and phone number.
        /// </summary>
        /// <param name="newDriver">The driver object to create.</param>
        /// <returns>A <see cref="CreateDriverResult"/> object containing the result of the creation operation.</returns>
        public async Task<CreateDriverResult> CreateAsync(Driver newDriver)
        {
            newDriver.Id = "";

            newDriver.Email = newDriver.Email.ToLower();

            var driverResult = new CreateDriverResult();

            // Validate the email address
            var emailValidation = DriverHelper.EmailValidation(newDriver);

            if (emailValidation)
            {
                driverResult.EmailValid = true;
            }

            // Check if the email address is available
            var emailAvailable = await GetByEmail(newDriver.Email);

            driverResult.EmailAvailable = emailAvailable == null;

            // Validate the phone number
             var phoneNumberValidation = DriverHelper.PhoneNumberValidation(newDriver);

            if (phoneNumberValidation)
            {
                driverResult.PhoneNumberValid = true;
            }

            // Check if all validations passed and if the email is available
            if (
                driverResult.EmailValid && 
                driverResult.EmailAvailable && 
                driverResult.PhoneNumberValid
            ) 
            {
                // Insert the new driver into the collection
                await _driversCollection.InsertOneAsync(newDriver);
                driverResult.Success = true;
            }


            // Return the result of the creation operation
            return driverResult;
        }

        public string? Authenticate(string email, string personNumber, string role)
        {
            var driver = _driversCollection.Find(x => x.Email == email && x.PersonNumber == personNumber).FirstOrDefault();

            if (driver is null)
            {
                return null;
            }

            var newAuuhentication = _createAuthentication;

            return newAuuhentication.CreateNewAuthen(email, role);

        }


        public async Task UpdateAsync(string id, Driver updatedDriver) =>
            await _driversCollection.ReplaceOneAsync(x => x.Id == id, updatedDriver);

        public async Task DeleteAsync(string id) =>
            await _driversCollection.DeleteOneAsync(x => x.Id == id);
        
    }
}
