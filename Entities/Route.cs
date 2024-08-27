using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    internal class Route
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RouteId { get; set; } = null!;
        public string RouteName { get; set; } = null!;
        public string DriverID { get; set; } = null!;
        public string DriverName { get; set; } = null!;
        public List<Order> Orders { get; set; } = null!; //List of orders objects I'll get this latre from another orders API. 

    }
}
