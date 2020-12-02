using MediatR;
using MongoDB.Bson;
using Moq;
using Printly.Printers;
using Printly.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Printers
{
    public class PrintersControllerTests
    {
        [Fact]
        public async Task GivenNoParams_WhenGet_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);

            var response = new GetAllPrintersQueryResponse
            {
                Printers = new List<Dto.Response.Printer>
                {
                    new Dto.Response.Printer
                    {
                        Id = "Hello"
                    },
                    new Dto.Response.Printer
                    {
                        Id = "World"
                    }
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetAllPrintersQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Get(cancellationTokenSource.Token);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.IsAny<GetAllPrintersQuery>(),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
            Assert.Equal(response.Printers.Count, result.Value.Count);
            Assert.Equal(response.Printers[0].Id, result.Value[0].Id);
            Assert.Equal(response.Printers[1].Id, result.Value[1].Id);
        }

        [Fact]
        public async Task GivenWhiteSpaceId_WhenGet_ThenBadRequestResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);

            var id = "  ";
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Get(
                id,
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenInvalidId_WhenGet_ThenBadRequestResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);

            var id = "pop";
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Get(
                id,
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenId_WhenGet_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);

            var id = ObjectId.GenerateNewId().ToString();
            var response = new GetPrinterByIdQueryResponse
            {
                Printer = new Dto.Response.Printer
                {
                    Id = "HelloWorld"
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetPrinterByIdQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Get(
                id,
                cancellationTokenSource.Token);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.Is<GetPrinterByIdQuery>(y => y.Id == id),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
            Assert.Equal(response.Printer.Id, result.Value.Id);
        }

        [Fact]
        public async Task GivenPrinter_WhenCreate_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var printer = new Dto.Request.Printer();
            var response = new CreatePrinterCommandResponse
            {
                Printer = new Dto.Response.Printer
                {
                    Id = "HelloWorld"
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<CreatePrinterCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Create(
                printer,
                cancellationTokenSource.Token);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.Is<CreatePrinterCommand>(y => y.Printer == printer),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
            Assert.Equal(response.Printer.Id, result.Value.Id);
        }

        [Fact]
        public async Task GivenModelStateError_WhenCreate_ThenBadRequestResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            sut.ModelState.AddModelError("error", "Something bad went down!");
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Create(
                new Dto.Request.Printer(),
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenId_AndPrinter_WhenUpdate_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var id = ObjectId.GenerateNewId().ToString();
            var printer = new Dto.Request.Printer();
            var response = new UpdatePrinterCommandResponse();

            mockMediator.Setup(x => x.Send(
                It.IsAny<UpdatePrinterCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Update(
                id,
                printer,
                cancellationTokenSource.Token);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.Is<UpdatePrinterCommand>(y => y.Id == id && y.Printer == printer),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GivenWhiteSpaceId_AndPrinter_WhenUpdate_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Update(
                " ",
                new Dto.Request.Printer(),
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenInvalidId_AndPrinter_WhenUpdate_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Update(
                "HelloWorld",
                new Dto.Request.Printer(),
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenModelStateError_WhenUpdate_ThenBadRequestResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            sut.ModelState.AddModelError("error", "Something bad went down!");
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Update(
                ObjectId.GenerateNewId().ToString(),
                new Dto.Request.Printer(),
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenId_WhenDelete_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var id = ObjectId.GenerateNewId().ToString();
            var response = new DeletePrinterByIdCommandResponse();

            mockMediator.Setup(x => x.Send(
                It.IsAny<DeletePrinterByIdCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Delete(
                id,
                cancellationTokenSource.Token);

            // Assert
            mockMediator.Verify(x => x.Send(
                It.Is<DeletePrinterByIdCommand>(y => y.Id == id),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GivenInvalidId_WhenDelete_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var response = new DeletePrinterByIdCommandResponse();

            mockMediator.Setup(x => x.Send(
                It.IsAny<DeletePrinterByIdCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Delete(
                "Hello World",
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }

        [Fact]
        public async Task GivenWhiteSpaceId_WhenDelete_ThenQuerySent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new PrintersController(
                new PrinterIdValidator(),
                mockMediator.Object);
            var response = new DeletePrinterByIdCommandResponse();

            mockMediator.Setup(x => x.Send(
                It.IsAny<DeletePrinterByIdCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.Delete(
                " ",
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(HttpStatusCode.BadRequest, result.Error.HttpStatusCode);
        }
    }
}
