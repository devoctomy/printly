using AutoMapper;
using MongoDB.Driver;
using Moq;
using Printly.Domain.Services;
using Printly.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Printers
{
    public class DeletePrinterByIdCommandHandlerTests
    {
        [Fact]
        public async Task GivenCommand_WhenHandled_ThenStorageDeleteCalled_AndResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var sut = new DeletePrinterByIdCommandHandler(mockDataStorage.Object);

            var command = new DeletePrinterByIdCommand()
            {
                Id = Guid.NewGuid().ToString()
            };

            mockDataStorage.Setup(x => x.Remove(
                It.IsAny<string>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(0));

            // Act
            var result = await sut.Handle(
                command,
                CancellationToken.None);

            // Assert
            mockDataStorage.Verify(x => x.Remove(
                It.Is<string>(y => y == command.Id)), Times.Once);
            Assert.True(result.IsAcknowledged);
        }
    }
}
