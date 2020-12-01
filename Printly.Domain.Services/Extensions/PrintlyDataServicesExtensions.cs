using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Printly.Domain.Models;
using Printly.Domain.Services.System;

namespace Printly.Domain.Services.Extensions
{
    public static class PrintlyDataServicesExtensions
    {
        public static IServiceCollection AddPrintlyDataServices(
            this IServiceCollection services,
            MongoDbConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(new MongoClient(configuration.ConnectionString));

            services.AddSingleton<MongoDbStorageServiceConfiguration<Printer>>(new MongoDbStorageServiceConfiguration<Printer>
            {
                DatabaseName = configuration.DatabaseName,
                CollectionName = "Printers"
            });
            services.AddSingleton<IDataStorageService<Printer>, PrinterStorageService>();

            services.AddSingleton<MongoDbStorageServiceConfiguration<Configuration>>(new MongoDbStorageServiceConfiguration<Configuration>
            {
                DatabaseName = configuration.DatabaseName,
                CollectionName = "Configuration"
            });
            services.AddSingleton<IDataStorageService<Configuration>, ConfigurationDataStorageService>();

            return services;
        }
    }
}
