using Microsoft.Extensions.DependencyInjection;
using Printly.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.Client.IntTests
{
    public class SystemClientTests
    {
        private readonly IServiceProvider _serviceProvider;

        public SystemClientTests()
        {
            var config = new PrintlyClientConfiguration
            {
                BaseUrl = "http://localhost:5000",
                HttpMessageHandlerLifetime = new TimeSpan(1, 0, 0),
                RetryCount = 3,
                SleepDuration = new TimeSpan(0, 0, 30)
            };
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddPrintlyClient(config);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task GivenCancellationToken_WhenGetInfoAsync_ThenApiCalled_AndResponseReturned()
        {
            // Arrange
            var systemClient = _serviceProvider.GetService<ISystemClient>();
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var response = await systemClient.GetInfoAsync(cancellationTokenSource.Token);

            // Assert
            Assert.True(response.Success);
        }

    }
}
