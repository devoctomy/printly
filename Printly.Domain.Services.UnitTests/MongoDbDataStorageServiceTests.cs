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
        private readonly MongoDbStorageServiceConfiguration<TestableStorageEntity> _configuration;
        private readonly Mock<IMongoClient> _mockMongoClient;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoCollection<TestableStorageEntity>> _mockCollection;

        public MongoDbDataStorageServiceTests()
        {
            _configuration = new MongoDbStorageServiceConfiguration<TestableStorageEntity>
            {
                DatabaseName = "Hello",
                CollectionName = "World"
            };
            _mockMongoClient = new Mock<IMongoClient>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<TestableStorageEntity>>();

            _mockMongoClient.Setup(x => x.GetDatabase(
                It.IsAny<string>(),
                It.IsAny<MongoDatabaseSettings>()))
                .Returns(_mockDatabase.Object);

            _mockDatabase.Setup(x => x.GetCollection<TestableStorageEntity>(
                It.IsAny<string>(),
                It.IsAny<MongoCollectionSettings>()))
                .Returns(_mockCollection.Object);
        }

        [Fact]
        public void GivenService_WhenConstruct_ThenGetDatabase_AndGetEntities()
        {
            // Arrange
            // Nothing to arrange

            // Act
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            // Assert
            Assert.NotNull(sut);
            _mockMongoClient.Verify(x => x.GetDatabase(
                It.Is<string>(y => y == _configuration.DatabaseName),
                It.IsAny<MongoDatabaseSettings>()), Times.Once);
            _mockDatabase.Verify(x => x.GetCollection<TestableStorageEntity>(
                It.Is<string>(y => y == _configuration.CollectionName),
                It.IsAny<MongoCollectionSettings>()), Times.Once);
        }

        [Fact]
        public async Task GivenCancellationToken_WhenGet_ThenCollectionFindAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Get(cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(),
                It.IsAny<FindOptions<TestableStorageEntity, TestableStorageEntity>>(),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndCancellationToken_WhenGet_ThenCollectionFindAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var id = ObjectId.GenerateNewId().ToString();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Get(
                id,
                cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<FindOptions<TestableStorageEntity, TestableStorageEntity>>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenPredicate_AndCancellationToken_WhenFind_ThenCollectionFindAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Find(
                y => true,
                cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.FindAsync(
                It.Is<FilterDefinition<TestableStorageEntity>>(y => true),
                It.Is<FindOptions<TestableStorageEntity, TestableStorageEntity>>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenEntity_AndCancellationToken_WhenCreate_ThenCollectionInsertOneAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var entity = new TestableStorageEntity();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Create(
                entity,
                cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.InsertOneAsync(
                It.Is<TestableStorageEntity>(y => y == entity),
                It.Is<InsertOneOptions>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndEntity_AndCancellationToken_WhenUpdate_ThenCollectionReplaceOneAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var entity = new TestableStorageEntity();
            var id = ObjectId.GenerateNewId().ToString();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Update(
                id,
                entity,
                cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.ReplaceOneAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<TestableStorageEntity>(y => y == entity),
                It.Is<ReplaceOptions>(y => y == null),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenEntity_AndCancellationToken_WhenRemove_ThenCollectionDeleteOneAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var entity = new TestableStorageEntity();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Remove(
                entity,
                cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.DeleteOneAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndCancellationToken_WhenRemove_ThenCollectionDeleteOneAsync()
        {
            // Arrange
            var sut = new TestableMongoDbDataStorageService(
                _mockMongoClient.Object,
                _configuration);

            var id = ObjectId.GenerateNewId().ToString();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Remove(
                id,
                cancellationTokenSource.Token);

            // Assert
            _mockCollection.Verify(x => x.DeleteOneAsync(
                It.IsAny<FilterDefinition<TestableStorageEntity>>(), // TODO: This needs asserting
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
