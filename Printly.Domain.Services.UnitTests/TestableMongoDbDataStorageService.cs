using MongoDB.Driver;

namespace Printly.Domain.Services.UnitTests
{
    public class TestableMongoDbDataStorageService : MongoDbDataStorageService<TestableStorageEntity>
    {
        public TestableMongoDbDataStorageService(
            IMongoClient mongoClient,
            MongoDbStorageServiceConfiguration<TestableStorageEntity> settings)
            : base(mongoClient, settings)
        {
        }
    }
}
