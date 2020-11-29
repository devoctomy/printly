using AutoMapper;
using Moq;
using Printly.Domain.Services;
using Printly.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Printers
{
    public class GetPrinterByIdQueryHandlerTests
    {
        [Fact]
        public async Task GivenCommand_AndPrinterId_AndPrinterExists_WhenHandled_ThenStorageGetCalled_AndDomainMappedToDto_AndResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var mockMapper = new Mock<IMapper>();
            var sut = new GetPrinterByIdQueryHandler(
                mockDataStorage.Object,
                mockMapper.Object);

            var query = new GetPrinterByIdQuery()
            {
                Id = Guid.NewGuid().ToString()
            };

            mockDataStorage.Setup(x => x.Get(
                It.IsAny<string>()))
                .ReturnsAsync(new Domain.Models.Printer());

            // Act
            var result = await sut.Handle(
                query,
                CancellationToken.None);

            // Assert
            mockDataStorage.Verify(x => x.Get(
                It.IsAny<string>()), Times.Once);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GivenCommand_AndPrinterId_AndPrinterNotExists_WhenHandled_ThenStorageGetCalled_AndDomainMappedToDto_AndResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var mockMapper = new Mock<IMapper>();
            var sut = new GetPrinterByIdQueryHandler(
                mockDataStorage.Object,
                mockMapper.Object);

            var query = new GetPrinterByIdQuery()
            {
                Id = Guid.NewGuid().ToString()
            };

            mockDataStorage.Setup(x => x.Get(
                It.IsAny<string>()))
                .ReturnsAsync((Domain.Models.Printer)null);

            // Act
            var result = await sut.Handle(
                query,
                CancellationToken.None);

            // Assert
            mockDataStorage.Verify(x => x.Get(
                It.IsAny<string>()), Times.Once);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.HttpStatusCode);
            Assert.Contains(query.Id, result.Error.Message);
        }
    }
}
