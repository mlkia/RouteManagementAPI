using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Destination
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DestinationId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AddressId { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public string DeliveryInstructions { get; set; } = null!;
    }
}
