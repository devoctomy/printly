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
                BaseUrl = "http://www.test.com",
                RetryCount = 10,
                SleepDuration = new System.TimeSpan(0,1,0),
                HttpMessageHandlerLifetime = new System.TimeSpan(0,10,0)
            };
            var sut = new ServiceCollection();

            // Act
            sut.AddPrintlyClient(config);
            var serviceProvider = sut.BuildServiceProvider();

            // Assert
            var systemClient = serviceProvider.GetServices<ISystemClient>();
            Assert.NotNull(systemClient);
        }
    }
}
