using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Threading;
using System.Threading.Tasks;
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
            Assert.NotNull(sut);
            mockMongoClient.Verify(x => x.GetDatabase(
                It.Is<string>(y => y == configuration.DatabaseName),
                It.IsAny<MongoDatabaseSettings>()), Times.Once);
            mockDatabase.Verify(x => x.GetCollection<TestableStorageEntity>(
                It.Is<string>(y => y == configuration.CollectionName),
                It.IsAny<MongoCollectionSettings>()), Times.Once);
        }

        [Fact]
        public async Task GivenCancellationToken_WhenGet_ThenCollectionFindAsync()
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Get(cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(),
                It.IsAny<FindOptions<TestableStorageEntity, TestableStorageEntity>>(),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndCancellationToken_WhenGet_ThenCollectionFindAsync()
        {
            // Arrange
            var configuration = new MongoDbStorageServiceConfiguration<TestableStorageEntity>
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var id = ObjectId.GenerateNewId().ToString();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Get(
                id,
                cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<FindOptions<TestableStorageEntity, TestableStorageEntity>>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenPredicate_AndCancellationToken_WhenFind_ThenCollectionFindAsync()
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Find(
                y => true,
                cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.FindAsync(
                It.Is<FilterDefinition<TestableStorageEntity>>(y => true),
                It.Is<FindOptions<TestableStorageEntity, TestableStorageEntity>>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenEntity_AndCancellationToken_WhenCreate_ThenCollectionInsertOneAsync()
        {
            // Arrange
            var configuration = new MongoDbStorageServiceConfiguration<TestableStorageEntity>
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var entity = new TestableStorageEntity();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Create(
                entity,
                cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.InsertOneAsync(
                It.Is<TestableStorageEntity>(y => y == entity),
                It.Is<InsertOneOptions>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndEntity_AndCancellationToken_WhenUpdate_ThenCollectionReplaceOneAsync()
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var entity = new TestableStorageEntity();
            var id = ObjectId.GenerateNewId().ToString();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Update(
                id,
                entity,
                cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.ReplaceOneAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<TestableStorageEntity>(y => y == entity),
                It.Is<ReplaceOptions>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenEntity_AndCancellationToken_WhenRemove_ThenCollectionDeleteOneAsync()
        {
            // Arrange
            var configuration = new MongoDbStorageServiceConfiguration<TestableStorageEntity>
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var entity = new TestableStorageEntity();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Remove(
                entity,
                cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.DeleteOneAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndCancellationToken_WhenRemove_ThenCollectionDeleteOneAsync()
        {
            // Arrange
            var configuration = new MongoDbStorageServiceConfiguration<TestableStorageEntity>
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

            var sut = new TestableMongoDbDataStorageService(
                mockMongoClient.Object,
                configuration);

            var id = ObjectId.GenerateNewId().ToString();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Remove(
                id,
                cancellationTokenSource.Token);

            // Assert
            mockCollection.Verify(x => x.DeleteOneAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
