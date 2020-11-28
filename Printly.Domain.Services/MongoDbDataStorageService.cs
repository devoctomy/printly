using MongoDB.Bson;
using MongoDB.Driver;
using Printly.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Printly.Domain.Services
{
    public abstract class MongoDbDataStorageService<T> : IDataStorageService<T> where T : StorageEntityBase
    {
        private readonly IMongoCollection<T> _entities;

        public MongoDbDataStorageService(MongoDbStorageServiceConfiguration settings)
        {
            if(!string.IsNullOrEmpty(settings.ConnectionString))
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);
                _entities = database.GetCollection<T>(settings.CollectionName);
            }
        }

        public async Task<List<T>> Get() =>
            (await _entities?.FindAsync(book => true)).ToList();

        
        public async Task<T> Get(string id) =>
            (await _entities?.FindAsync(entity => entity.Id == ObjectId.Parse(id))).FirstOrDefault();

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter) =>
            (await _entities?.FindAsync(filter)).ToEnumerable();

        public async Task Create(T entity) =>
            await _entities?.InsertOneAsync(entity);

        public Task<ReplaceOneResult> Update(string id, T entity) =>
            _entities?.ReplaceOneAsync(book => book.Id == ObjectId.Parse(id), entity);

        public Task<DeleteResult> Remove(T entity) =>
            _entities?.DeleteOneAsync(book => book.Id == entity.Id);

        public Task<DeleteResult> Remove(string id) =>
            _entities?.DeleteOneAsync(book => book.Id == ObjectId.Parse(id));
    }
}
