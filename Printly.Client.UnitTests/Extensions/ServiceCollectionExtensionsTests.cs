using Microsoft.Extensions.DependencyInjection;
using Printly.Client.Extensions;
using Xunit;

namespace Printly.Client.UnitTests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void GivenConfig_WhenAddPrintlyClient_ThenClientAddedForInjection()
        {
            // Arrange
            var config = new PrintlyClientConfiguration()
            {
                BaseUrl = "http://www.test.com/",
                RetryCount = 10,
                SleepDuration = new System.TimeSpan(0,1,0),
                HttpMessageHandlerLifetime = new System.TimeSpan(0,10,0)
            };
            var sut = new ServiceCollection();

            // Act
            sut.AddPrintlyClient(config);
            var serviceProvider = sut.BuildServiceProvider();

            // Assert
            var systemClientHttpAdapter = serviceProvider.GetService<IHttpAdapter<SystemClient>>();
            Assert.Equal(config.BaseUrl, systemClientHttpAdapter.BaseUrl);
            Assert.NotNull(serviceProvider.GetServices<ISystemClient>());
            var printersClientHttpAdapter = serviceProvider.GetService<IHttpAdapter<PrintersClient>>();
            Assert.Equal(config.BaseUrl, printersClientHttpAdapter.BaseUrl);
            Assert.NotNull(serviceProvider.GetServices<IPrintersClient>());
        }
    }
}
