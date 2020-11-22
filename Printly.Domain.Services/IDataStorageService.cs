using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Printly.Domain.Services
{
    public interface IDataStorageService<T>
    {
        List<T> Get();
        T Get(string id);
        IEnumerable<T> Find(Expression<Func<T, bool>> filter);
        T Create(T sample);
        void Update(string id, T sample);
        void Remove(T sample);
        void Remove(string id);
    }
}
