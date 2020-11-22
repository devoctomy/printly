using MongoDB.Driver;
using Printly.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Printly.Domain.Services
{
    public class MongoDbDataStorageService<T> : IDataStorageService<T> where T : StorageEntityBase
    {
        private readonly IMongoCollection<T> _entities;

        public MongoDbDataStorageService(MongoDbStorageServiceConfiguration settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _entities = database.GetCollection<T>(settings.CollectionName);
        }

        public List<T> Get() =>
            _entities.Find(book => true).ToList();

        public T Get(string id) =>
            _entities.Find(entity => entity.Id == id).FirstOrDefault();

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter) =>
            _entities.Find(filter).ToEnumerable();

        public T Create(T entity)
        {
            _entities.InsertOne(entity);
            return entity;
        }

        public void Update(string id, T entity) =>
            _entities.ReplaceOne(book => book.Id == id, entity);

        public void Remove(T entity) =>
            _entities.DeleteOne(book => book.Id == entity.Id);

        public void Remove(string id) =>
            _entities.DeleteOne(book => book.Id == id);
    }
}
