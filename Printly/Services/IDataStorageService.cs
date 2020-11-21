using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printly.Services
{
    public interface IDataStorageService
    {
        bool Insert<T>(T value);
        bool Update<T>(T value);
        bool Delete<T>(T value);
    }
}
