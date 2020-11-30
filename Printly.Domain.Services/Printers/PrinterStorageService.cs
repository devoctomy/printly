using MongoDB.Driver;
using Printly.Domain.Models;

namespace Printly.Domain.Services.System
{
    public class PrinterStorageService : MongoDbDataStorageService<Printer>
    {
        public PrinterStorageService(
            IMongoClient mongoClient,
            MongoDbStorageServiceConfiguration<Printer> settings)
            :base(mongoClient, settings)
        {
        }
    }
}
