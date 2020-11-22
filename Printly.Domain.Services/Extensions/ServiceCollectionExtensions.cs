using Microsoft.Extensions.DependencyInjection;
using Printly.Domain.Models;

namespace Printly.Domain.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPrintlyDataServices(
            this IServiceCollection services,
            MongoDbConfiguration configuration)
        {
            services.AddSingleton<IDataStorageService<Printer>>(new MongoDbDataStorageService<Printer>(new MongoDbStorageServiceConfiguration()
            {
                ConnectionString = configuration.ConnectionString,
                DatabaseName = configuration.DatabaseName,
                CollectionName = "Printers"
            }));
            return services;
        }
    }
}
