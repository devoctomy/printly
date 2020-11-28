using Microsoft.Extensions.DependencyInjection;
using Printly.Domain.Models;
using Printly.Domain.Services.System;

namespace Printly.Domain.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPrintlyDataServices(
            this IServiceCollection services,
            MongoDbConfiguration configuration)
        {
            services.AddSingleton<IDataStorageService<Printer>>(new PrinterStorageService(new MongoDbStorageServiceConfiguration()
            {
                ConnectionString = configuration.ConnectionString,
                DatabaseName = configuration.DatabaseName,
                CollectionName = "Printers"
            }));

            services.AddSingleton<IDataStorageService<Configuration>>(new ConfigurationDataStorageService(new MongoDbStorageServiceConfiguration()
            {
                ConnectionString = configuration.ConnectionString,
                DatabaseName = configuration.DatabaseName,
                CollectionName = "Configuration"
            }));

            return services;
        }
    }
}
