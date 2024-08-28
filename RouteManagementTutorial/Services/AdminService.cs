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
        private readonly JwtSettings _jwtSettings;

        public AdminService(IOptions<RouteManagementTutorialDataBaseSettings> RMTDataBasesettings, IOptions<JwtSettings> jwtSettings)
        {
            var mongoclient = new MongoClient(RMTDataBasesettings.Value.ConnectionString);
            var database = mongoclient.GetDatabase(RMTDataBasesettings.Value.DatabaseName);

            _adminsCollection = database.GetCollection<Admin>(RMTDataBasesettings.Value.AdminsCollectionName);
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<List<Admin>> GetAsync() => 
            await _adminsCollection.Find(_ => true).ToListAsync();

        public async Task<Admin?> GetAsync(string id) =>
            await _adminsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Admin newAdmin) =>
            await _adminsCollection.InsertOneAsync(newAdmin);


        public string? Authenticate(string email, string password)
        {
            var user = _adminsCollection.Find(x => x.Email == email && x.Password == password).FirstOrDefault();

            if (user is null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
