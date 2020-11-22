using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printly
{
    public class AppSettings
    {
        public string ServiceId { get; set; }
        public string MongoDbStorageConnectionString { get; set; }
        public string MongoDbStorageDatabaseName { get; set; }
    }
}
