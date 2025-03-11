using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Stop
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? StopId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? DestinationId { get; set; }
        public Destination Destination { get; set; } = null!;
        public DateTime EstimatedArrivalTime { get; set; }
        public DateTime? ActualArrivalTime { get; set; }
        public string Status { get; set; } = null!;
    }
}
