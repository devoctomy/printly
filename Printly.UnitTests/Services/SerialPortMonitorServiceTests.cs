using Moq;
using Printly.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class SerialPortMonitorServiceTests
    {
        [Fact]
        public async Task Given2PortsConnected_WhenStart_ThenPortConnectedEventsRaised()
        {
            // Arrange
            var mockSerialPortDiscoveryService = new Mock<ISerialPortDiscoveryService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var sut = new SerialPortMonitorService(
                mockSerialPortDiscoveryService.Object,
                mockDateTimeService.Object);

            mockSerialPortDiscoveryService.Setup(x => x.GetPorts())
                .Returns(new [] { "COM1", "COM2" });

            mockDateTimeService.SetupGet(x => x.UtcNow).Returns(DateTime.UtcNow);

            var currentlyConnectedPorts = new List<string>();

            sut.PortsConnected += (sender, e) =>
            {
                currentlyConnectedPorts.AddRange(e.SerialPorts);
            };

            sut.PortsDisconnected += (sender, e) =>
            {
                foreach(var curPort in e.SerialPorts)
                {
                    currentlyConnectedPorts.Remove(curPort);
                }
            };

            // Act
            sut.Start();
            await Task.Delay(5000).ConfigureAwait(false);

            // Assert
            Assert.Equal(2, currentlyConnectedPorts.Count);
            Assert.Contains(currentlyConnectedPorts, x => x == "COM1");
            Assert.Contains(currentlyConnectedPorts, x => x == "COM2");
        }

        [Fact]
        public async Task GivenPortConnectionActivity_WhenRunning_ThenCorrectListOfConnectedPortsMaintained()
        {
            // Arrange
            var mockSerialPortDiscoveryService = new Mock<ISerialPortDiscoveryService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var sut = new SerialPortMonitorService(
                mockSerialPortDiscoveryService.Object,
                mockDateTimeService.Object);

            mockDateTimeService.SetupGet(x => x.UtcNow).Returns(DateTime.UtcNow);

            var currentlyConnectedPorts = new List<string>();

            sut.PortsConnected += (sender, e) =>
            {
                currentlyConnectedPorts.AddRange(e.SerialPorts);
            };

            sut.PortsDisconnected += (sender, e) =>
            {
                foreach (var curPort in e.SerialPorts)
                {
                    currentlyConnectedPorts.Remove(curPort);
                }
            };

            // Act
            sut.Start();
            await PortConnectionActivity(mockSerialPortDiscoveryService).ConfigureAwait(false);

            // Assert
            Assert.Equal(2, currentlyConnectedPorts.Count);
            Assert.Contains(currentlyConnectedPorts, x => x == "COM3");
            Assert.Contains(currentlyConnectedPorts, x => x == "COM4");
        }

        private static async Task PortConnectionActivity(Mock<ISerialPortDiscoveryService> mockSerialPortDiscoveryService)
        {
            await Task.Yield();
            var connectedPorts = new List<string> { "COM1", "COM2" };
            mockSerialPortDiscoveryService.Setup(x => x.GetPorts())
                .Returns(() =>
                {
                    return connectedPorts.ToArray();
                });

            await Task.Delay(1000).ConfigureAwait(false);
            connectedPorts.Remove("COM1");

            await Task.Delay(1000).ConfigureAwait(false);
            connectedPorts.Remove("COM2");

            await Task.Delay(1000).ConfigureAwait(false);
            connectedPorts.Add("COM3");

            await Task.Delay(1000).ConfigureAwait(false);
            connectedPorts.Add("COM4");

            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}
