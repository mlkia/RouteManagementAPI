using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace RouteManagementTutorial.Entities
{
    public class Driver
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("DriverName")]
        public string Name { get; set; } = null!;
        public int PersonNumber { get; set; }
        public int   PhoneNumber { get; set; } 
        public string Email { get; set; } = null!;
        public string LicenseType { get; set; } = null!;
    }
     
}
