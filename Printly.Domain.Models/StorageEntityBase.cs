using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Printly.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class StorageEntityBase : IStorageEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual ObjectId Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
