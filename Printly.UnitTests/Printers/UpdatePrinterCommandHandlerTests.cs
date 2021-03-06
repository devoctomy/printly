﻿using AutoMapper;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Driver;
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
    public class UpdatePrinterCommandHandlerTests
    {
        [Fact]
        public async Task GivenCommand_AndPrinterExists_WhenHandled_ThenDtoMappedToDomain_AndStorageUpdateCalled_AndResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var mockMapper = new Mock<IMapper>();
            var sut = new UpdatePrinterCommandHandler(
                mockDataStorage.Object,
                mockMapper.Object,
                Mock.Of<IStringLocalizerFactory>());

            var command = new UpdatePrinterCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Printer = new Dto.Request.Printer()
            };

            var printerDomain = new Domain.Models.Printer();

            mockMapper.Setup(x => x.Map<Domain.Models.Printer>(
                It.IsAny<Dto.Request.Printer>())).Returns(printerDomain);

            mockDataStorage.Setup(x => x.Update(
                It.IsAny<string>(),
                It.IsAny<Domain.Models.Printer>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(
                    1,
                    1,
                    null));

            // Act
            var response = await sut.Handle(
                command,
                CancellationToken.None);

            // Assert
            mockMapper.Verify(x => x.Map<Domain.Models.Printer>(
                It.Is<Dto.Request.Printer>(y => y == command.Printer)), Times.Once);
            mockDataStorage.Verify(x => x.Update(
                It.Is<string>(y => y == command.Id),
                It.Is<Domain.Models.Printer>(y => y == printerDomain),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.Null(response.Error);
        }

        [Fact]
        public async Task GivenCommand_AndPrinterNotExists_WhenHandled_ThenDtoMappedToDomain_AndStorageUpdateCalled_AndErrorResponseReturned()
        {
            // Arrange
            var mockDataStorage = new Mock<IDataStorageService<Domain.Models.Printer>>();
            var mockMapper = new Mock<IMapper>();
            var mockStringLocalizerFactory = new Mock<IStringLocalizerFactory>();
            var mockStringLocalizer = new Mock<IStringLocalizer>();

            mockStringLocalizerFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(mockStringLocalizer.Object);

            var sut = new UpdatePrinterCommandHandler(
                mockDataStorage.Object,
                mockMapper.Object,
                mockStringLocalizerFactory.Object);

            var command = new UpdatePrinterCommand
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Printer = new Dto.Request.Printer()
            };

            var printerDomain = new Domain.Models.Printer();

            mockMapper.Setup(x => x.Map<Domain.Models.Printer>(
                It.IsAny<Dto.Request.Printer>())).Returns(printerDomain);

            mockDataStorage.Setup(x => x.Update(
                It.IsAny<string>(),
                It.IsAny<Domain.Models.Printer>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(
                    0,
                    null,
                    null));

            mockStringLocalizer.SetupGet(x => x["PrinterNotFound", It.IsAny<string>()]).Returns(new LocalizedString("PrinterNotFound", $"{command.Id}"));

            // Act
            var result = await sut.Handle(
                command,
                CancellationToken.None);

            // Assert
            mockMapper.Verify(x => x.Map<Domain.Models.Printer>(
                It.Is<Dto.Request.Printer>(y => y == command.Printer)), Times.Once);
            mockDataStorage.Verify(x => x.Update(
                It.Is<string>(y => y == command.Id),
                It.Is<Domain.Models.Printer>(y => y == printerDomain),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result.Error);
            Assert.Equal(HttpStatusCode.NotFound, result.Error.HttpStatusCode);
            Assert.Contains(command.Id, result.Error.Message);
        }
    }
}
