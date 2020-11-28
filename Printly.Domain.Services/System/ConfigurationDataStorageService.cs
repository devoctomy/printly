using Printly.Domain.Models;

namespace Printly.Domain.Services.System
{
    public class ConfigurationDataStorageService : MongoDbDataStorageService<Configuration>
    {
        public ConfigurationDataStorageService(MongoDbStorageServiceConfiguration settings)
            :base(settings)
        {
        }
    }
}
