using AutoMapper;
using Moq;
using Printly.Domain.Services;
using Printly.Mapping;
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
    public class CreatePrinterCommandHandlerTests
    {
        [Fact]
        public async Task GivenCommand_WhenHandled_ThenDtoMappedToDomain_AndStorageCreateCalled_AndDomainMappedBackToDto_AndResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var mockMapper = new Mock<IMapper>();
            var sut = new CreatePrinterCommandHandler(
                mockDataStorage.Object,
                mockMapper.Object);

            var command = new CreatePrinterCommand
            {
                Printer = new Dto.Request.Printer()
            };

            var printerDomain = new Domain.Models.Printer();
            var printerResponse = new Dto.Response.Printer();

            mockMapper.Setup(x => x.Map<Domain.Models.Printer>(
                It.IsAny<Dto.Request.Printer>())).Returns(printerDomain);

            mockMapper.Setup(x => x.Map<Dto.Response.Printer>(
                It.IsAny<Domain.Models.Printer>())).Returns(printerResponse);

            mockDataStorage.Setup(x => x.Create(
                It.IsAny<Domain.Models.Printer>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await sut.Handle(
                command,
                CancellationToken.None);

            // Assert
            mockMapper.Verify(x => x.Map<Domain.Models.Printer>(
                It.Is<Dto.Request.Printer>(y => y == command.Printer)), Times.Once);

            mockMapper.Verify(x => x.Map<Dto.Response.Printer>(
                It.Is<Domain.Models.Printer>(y => y == printerDomain)), Times.Once);

            mockDataStorage.Verify(x => x.Create(
                It.Is<Domain.Models.Printer>(y => y == printerDomain),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
