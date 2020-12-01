using AutoMapper;
using Moq;
using Printly.Domain.Services;
using Printly.Printers;
using System;
using System.Net;
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
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Models.Printer());

            mockMapper.Setup(x => x.Map<Dto.Response.Printer>(
                It.IsAny<object>()))
                .Returns(new Dto.Response.Printer
                {
                    Name = "Bob Hoskins"
                });

            // Act
            var result = await sut.Handle(
                query,
                CancellationToken.None);

            // Assert
            mockDataStorage.Verify(x => x.Get(
                It.Is<string>(y => y == query.Id),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Null(result.Error);
            Assert.Equal("Bob Hoskins", result.Printer.Name);
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
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Models.Printer)null);

            // Act
            var result = await sut.Handle(
                query,
                CancellationToken.None);

            // Assert
            mockDataStorage.Verify(x => x.Get(
                It.Is<string>(y => y == query.Id),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.HttpStatusCode);
            Assert.Contains(query.Id, result.Error.Message);
        }
    }
}
