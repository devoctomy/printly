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
    public class GetAllPrintersQueryHandlerTests
    {
        [Fact]
        public async Task GivenCommand_AndPrintersExists_WhenHandled_ThenStorageGetCalled_AndDomainMappedToDto_AndResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var mockMapper = new Mock<IMapper>();
            var sut = new GetAllPrintersQueryHandler(
                mockDataStorage.Object,
                mockMapper.Object);

            var query = new GetAllPrintersQuery();

            mockDataStorage.Setup(x => x.Get(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Models.Printer>());

            mockMapper.Setup(x => x.Map<List<Dto.Response.Printer>>(
                It.IsAny<object>()))
                .Returns(new List<Dto.Response.Printer>()
                {
                    new Dto.Response.Printer(),
                    new Dto.Response.Printer()
                });

            // Act
            var result = await sut.Handle(
                query,
                CancellationToken.None);

            // Assert
            mockDataStorage.Verify(x => x.Get(
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Null(result.Error);
            Assert.Equal(2, result.Printers.Count);
        }
    }
}
