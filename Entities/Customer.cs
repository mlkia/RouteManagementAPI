using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("CustomerName")]
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
    }
}
