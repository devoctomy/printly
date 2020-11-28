using MediatR;
using MongoDB.Bson;
using Moq;
using Printly.Printers;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Printly.UnitTests.Printers
{
    public class PrintersControllerTests
    {
        [Fact]
        public async void GivenNoParams_WhenGet_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(mockMediator.Object);

            var response = new GetAllPrintersQueryResponse()
            {
                Printers = new List<Dto.Response.Printer>()
                {
                    new Dto.Response.Printer()
                    {
                        Id = "Hello"
                    },
                    new Dto.Response.Printer()
                    {
                        Id = "World"
                    }
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetAllPrintersQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.Get(CancellationToken.None);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.IsAny<GetAllPrintersQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(response.Printers.Count, result.Printers.Count);
            Assert.Equal(response.Printers[0].Id, result.Printers[0].Id);
            Assert.Equal(response.Printers[1].Id, result.Printers[1].Id);
        }

        [Fact]
        public async void GivenId_WhenGet_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(mockMediator.Object);

            var id = ObjectId.GenerateNewId().ToString();
            var response = new GetPrinterByIdQueryResponse()
            {
                Printer = new Dto.Response.Printer()
                {
                    Id = "HelloWorld"
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetPrinterByIdQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.Get(
                id,
                CancellationToken.None);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.IsAny<GetPrinterByIdQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(response.Printer.Id, result.Printer.Id);
        }

        [Fact]
        public async void GivenPrinter_WhenCreate_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(mockMediator.Object);

            var response = new CreatePrinterCommandResponse()
            {
                Printer = new Dto.Response.Printer()
                {
                    Id = "HelloWorld"
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<CreatePrinterCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.Create(
                new Dto.Request.Printer(),
                CancellationToken.None);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.IsAny<CreatePrinterCommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(response.Printer.Id, result.Printer.Id);
        }

        [Fact]
        public async void GivenId_AndPrinter_WhenCreate_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(mockMediator.Object);

            var response = new UpdatePrinterCommandResponse()
            {
                IsAcknowledged = true
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<UpdatePrinterCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.Update(
                "HelloWorld",
                new Dto.Request.Printer(),
                CancellationToken.None);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.IsAny<UpdatePrinterCommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(response.IsAcknowledged);
        }

        [Fact]
        public async void GivenId_WhenDelete_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(mockMediator.Object);

            var response = new DeletePrinterByIdCommandResponse()
            {
                IsAcknowledged = true
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<DeletePrinterByIdCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.Delete(
                "HelloWorld",
                CancellationToken.None);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.IsAny<DeletePrinterByIdCommand>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(response.IsAcknowledged);
        }
    }
}
