namespace Printly.Domain.Services
{
    public class MongoDbStorageServiceConfiguration<T>
    {
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
