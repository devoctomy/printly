namespace Printly.Domain.Services
{
    public class MongoDbStorageServiceConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
