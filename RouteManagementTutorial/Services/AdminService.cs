using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RouteManagementTutorial.Models;
using RouteManagementTutorial.Entities;
using RouteManagementTutorial.Helper;
using RouteManagementTutorial.DTO;
using Entities;
using RouteManagementTutorial.Authenticate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RouteManagementTutorial.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<Admin> _adminsCollection;
        private readonly CreateAuthentication _createAythentication;

        public AdminService(IOptions<RouteManagementTutorialDataBaseSettings> RMTDataBasesettings, CreateAuthentication createAythentication)
        {
            var mongoclient = new MongoClient(RMTDataBasesettings.Value.ConnectionString);
            var database = mongoclient.GetDatabase(RMTDataBasesettings.Value.DatabaseName);

            _adminsCollection = database.GetCollection<Admin>(RMTDataBasesettings.Value.AdminsCollectionName);
            _createAythentication = createAythentication;
        }

        public async Task<List<Admin>> GetAsync() => 
            await _adminsCollection.Find(_ => true).ToListAsync();

        public async Task<Admin?> GetAsync(string id) =>
            await _adminsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves a admin by their email address.
        /// </summary>
        /// <param name="email">The email address of the admin.</param>
        /// <returns>The admin with the specified email address, or <c>null</c> if no such admin exists.</returns>
        public async Task<Admin?> GetByEmail(string email) =>
            await _adminsCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task<CreateResult> CreateAsync(Admin newAdmin)
        {
            newAdmin.Id = "";

            newAdmin.Email = newAdmin.Email.ToLower();

            var newAdminResult = new CreateResult();

            // Validate the email address
            var emailValidation = ValidationHelper.EmailValidation(newAdmin.Email);

            if (emailValidation)
            {
                newAdminResult.EmailValid = true;
            }

            // Check if the email address is available
            var emailAvailable = await GetByEmail(newAdmin.Email);

            // If emailAvailable is null, CreateResult.EmailAvailable is true, otherwise false.
            newAdminResult.EmailAvailable = emailAvailable == null;

            // Validate the password
            var passwordValidation = ValidationHelper.PasswordValidation(newAdmin.Password, "admin");

            if (passwordValidation)
            {
                newAdminResult.PasswordValid = true;
            }

            // Validate the phone number
            var phoneNumberValidation = ValidationHelper.PhoneNumberValidation(newAdmin.PhoneNumber);

            if (phoneNumberValidation)
            {
                newAdminResult.PhoneNumberValid = true;
            }


            //Check The first name, last name and the license type != null or empty.
            newAdminResult.FirstName = !string.IsNullOrEmpty(newAdmin.FirstName.Trim());
            newAdminResult.LastName = !string.IsNullOrEmpty(newAdmin.LastName.Trim());


            // Check if all validations passed and if the email is available
            if (
                newAdminResult.EmailValid &&
                newAdminResult.EmailAvailable &&
                newAdminResult.PhoneNumberValid &&
                newAdminResult.PasswordValid &&
                newAdminResult.FirstName &&
                newAdminResult.LastName 
            )
            {
                // Insert the new admin into the collection
                await _adminsCollection.InsertOneAsync(newAdmin);
                newAdminResult.Success = true;
            }


            // Return the result of the creation operation
            return newAdminResult;
        }

        public string? Authenticate(string email, string password, string role)
        {
            var user = _adminsCollection.Find(x => x.Email == email && x.Password == password).FirstOrDefault();

            if (user is null)
            {
                return null;
            }

            var newAuyhentication = _createAythentication;

            return newAuyhentication.CreateNewAuthen(email, role);

        }

    }
}
