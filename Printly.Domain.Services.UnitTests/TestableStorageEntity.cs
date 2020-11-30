using MongoDB.Bson;
using Printly.Domain.Models;

namespace Printly.Domain.Services.UnitTests
{
    public class TestableStorageEntity : StorageEntityBase
    {
        public override ObjectId Id { get; set; }
    }
}
