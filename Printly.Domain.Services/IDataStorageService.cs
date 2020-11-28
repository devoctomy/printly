using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Printly.Domain.Services
{
    public interface IDataStorageService<T>
    {
        Task<List<T>> Get();
        Task<T> Get(string id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter);
        Task Create(T sample);
        Task<ReplaceOneResult> Update(string id, T sample);
        DeleteResult Remove(T sample);
        DeleteResult Remove(string id);
    }
}
