using MongoDB.Driver;
using Printly.Domain.Models;

namespace Printly.Domain.Services.System
{
    public class ConfigurationDataStorageService : MongoDbDataStorageService<Configuration>
    {
        public ConfigurationDataStorageService(
            IMongoClient mongoClient,
            MongoDbStorageServiceConfiguration<Configuration> settings)
            :base(mongoClient, settings)
        {
        }
    }
}
