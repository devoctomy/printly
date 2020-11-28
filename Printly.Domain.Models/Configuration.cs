using MongoDB.Bson;
using System;

namespace Printly.Domain.Models
{
    public class Configuration : StorageEntityBase
    {
        public override ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string SystemId { get; set; } = Guid.NewGuid().ToString();
    }
}
