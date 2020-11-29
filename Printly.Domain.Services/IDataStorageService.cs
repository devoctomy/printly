using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Domain.Services
{
    public interface IDataStorageService<T>
    {
        Task<List<T>> Get(CancellationToken cancellationToken);
        Task<T> Get(
            string id,
            CancellationToken cancellationToken);
        Task<IEnumerable<T>> Find(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken);
        Task Create(
            T sample,
            CancellationToken cancellationToken);
        Task<ReplaceOneResult> Update(
            string id,
            T sample,
            CancellationToken cancellationToken);
        Task<DeleteResult> Remove(
            T sample,
            CancellationToken cancellationToken);
        Task<DeleteResult> Remove(
            string id,
            CancellationToken cancellationToken);
    }
}
