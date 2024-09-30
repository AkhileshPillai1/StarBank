using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.Models
{
    internal class User
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? EmailId { get; set; } = string.Empty;
        public string? PhoneNumber {  get; set; } = string.Empty;
        public double Balance { get; set; }
        public string? Password { get; set; } = string.Empty;
    }
}
