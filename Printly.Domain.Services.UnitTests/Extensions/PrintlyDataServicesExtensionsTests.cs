using Microsoft.Extensions.DependencyInjection;
using Printly.Domain.Models;
using Printly.Domain.Services.Extensions;
using Printly.Domain.Services.System;
using Xunit;

namespace Printly.Domain.Services.UnitTests.Extensions
{
    public class PrintlyDataServicesExtensionsTests
    {
        [Fact]
        public void GivenConfig_WhenAddPrintlyDataServices_ThenServicesAddedForInjection()
        {
            // Arrange
            var sut = new ServiceCollection();

            // Act
            sut.AddPrintlyDataServices(new MongoDbConfiguration
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "Test"
            });
            var serviceProvider = sut.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetServices<MongoDbStorageServiceConfiguration<Printer>>());
            Assert.NotNull(serviceProvider.GetServices<MongoDbStorageServiceConfiguration<Configuration>>());
            Assert.NotNull(serviceProvider.GetServices<PrinterStorageService>());
            Assert.NotNull(serviceProvider.GetServices<ConfigurationDataStorageService>());
        }
    }
}
