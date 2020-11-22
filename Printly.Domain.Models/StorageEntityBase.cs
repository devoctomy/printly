using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Printly.Domain.Models
{
    public class StorageEntityBase : IStorageEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual ObjectId Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
