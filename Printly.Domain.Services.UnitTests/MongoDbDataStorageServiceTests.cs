using MongoDB.Driver;
using Moq;
using Xunit;

namespace Printly.Domain.Services.UnitTests
{
    public class MongoDbDataStorageServiceTests
    {
        [Fact]
        public void GivenService_WhenConstruct_ThenGetDatabase_AndGetEntities()
        {
            // Arrange
            var configuration = new MongoDbStorageServiceConfiguration<TestableStorageEntity>()
            {
                DatabaseName = "Hello",
                CollectionName = "World"
            };
            var mockMongoClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockCollection = new Mock<IMongoCollection<TestableStorageEntity>>();

            mockMongoClient.Setup(x => x.GetDatabase(
                It.IsAny<string>(),
                It.IsAny<MongoDatabaseSettings>()))
                .Returns(mockDatabase.Object);

            mockDatabase.Setup(x => x.GetCollection<TestableStorageEntity>(
                It.IsAny<string>(),
                It.IsAny<MongoCollectionSettings>()))
                .Returns(mockCollection.Object);

            // Act
            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            // Assert
            mockMongoClient.Verify(x => x.GetDatabase(
                It.Is<string>(y => y == configuration.DatabaseName),
                It.IsAny<MongoDatabaseSettings>()), Times.Once);

            mockDatabase.Verify(x => x.GetCollection<TestableStorageEntity>(
                It.Is<string>(y => y == configuration.CollectionName),
                It.IsAny<MongoCollectionSettings>()), Times.Once);
        }
    }
}
