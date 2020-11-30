using MongoDB.Bson;
using MongoDB.Driver;
using Printly.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Domain.Services
{
    public abstract class MongoDbDataStorageService<T> : IDataStorageService<T> where T : StorageEntityBase
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> _entities;

        public MongoDbDataStorageService(
            IMongoClient mongoClient,
            MongoDbStorageServiceConfiguration<T> settings)
        {
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(settings.DatabaseName);
            _entities = _mongoDatabase.GetCollection<T>(settings.CollectionName);
        }

        public async Task<List<T>> Get(CancellationToken cancellationToken) =>
            (await _entities?.FindAsync(
                entity => true,
                null,
                cancellationToken)).ToList();

        
        public async Task<T> Get(
            string id,
            CancellationToken cancellationToken) =>
            (await _entities?.FindAsync(
                entity => entity.Id == ObjectId.Parse(id),
                null,
                cancellationToken)).FirstOrDefault();

        public async Task<IEnumerable<T>> Find(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken) =>
            (await _entities?.FindAsync(
                filter,
                null,
                cancellationToken)).ToEnumerable();

        public async Task Create(
            T entity,
            CancellationToken cancellationToken) =>
            await _entities?.InsertOneAsync(
                entity,
                null,
                cancellationToken);

        public Task<ReplaceOneResult> Update(
            string id,
            T entity,
            CancellationToken cancellationToken) =>
            _entities.ReplaceOneAsync(
                entity => entity.Id == ObjectId.Parse(id),
                entity,
                (ReplaceOptions)null,
                cancellationToken);

        public Task<DeleteResult> Remove(
            T entity,
            CancellationToken cancellationToken) =>
            _entities?.DeleteOneAsync(
                entity => entity.Id == entity.Id,
                cancellationToken);

        public Task<DeleteResult> Remove(
            string id,
            CancellationToken cancellationToken) =>
            _entities?.DeleteOneAsync(
                entity => entity.Id == ObjectId.Parse(id),
                cancellationToken);
    }
}
