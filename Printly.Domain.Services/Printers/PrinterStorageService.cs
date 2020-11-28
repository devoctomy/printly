using Printly.Domain.Models;

namespace Printly.Domain.Services.System
{
    public class PrinterStorageService : MongoDbDataStorageService<Printer>
    {
        public PrinterStorageService(MongoDbStorageServiceConfiguration settings)
            :base(settings)
        {
        }
    }
}
