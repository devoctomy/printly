using Microsoft.Extensions.Logging;
using Moq;
using Printly.Services;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class SerialPortCommunicationServiceFactoryTests
    {
        [Fact]
        public void GivenService_WhenCreate_ThenSerialPortCommunicationServiceReturned()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var sut = new SerialPortCommunicationServiceFactory(
                mockSerialPortFactory.Object,
                Mock.Of<ILogger<SerialPortCommunicationService>>());

            // Act
            var result = sut.Create();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SerialPortCommunicationService>(result);
        }
    }
}
