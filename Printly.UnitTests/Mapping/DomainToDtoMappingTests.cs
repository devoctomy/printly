using AutoMapper;
using MongoDB.Bson;
using Printly.Mapping;
using System;
using Xunit;

namespace Printly.UnitTests.Mapping
{
    public class DomainToDtoMappingTests
    {
        [Fact]
        public void GivenPrinterDomain_WhenMappedToPrinterDto_ThenMappingCorrect()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToDtoMapping>();
            });
            var sut = new Mapper(config);

            var domainPrinter = new Domain.Models.Printer
            {
                Id = ObjectId.GenerateNewId(),
                MarlinId = Guid.NewGuid().ToString(),
                Name = "Hello World",
                BedSizeX = 100,
                BedSizeY = 200,
                BedSizeZ = 300
            };

            // Act
            var dtoPrinter = sut.Map<Dto.Response.Printer>(domainPrinter);

            // Assert
            Assert.Equal(domainPrinter.Id.ToString(), dtoPrinter.Id);
            Assert.Equal(domainPrinter.MarlinId, dtoPrinter.MarlinId);
            Assert.Equal(domainPrinter.Name, dtoPrinter.Name);
            Assert.Equal(domainPrinter.BedSizeX, dtoPrinter.BedSize.X);
            Assert.Equal(domainPrinter.BedSizeY, dtoPrinter.BedSize.Y);
            Assert.Equal(domainPrinter.BedSizeZ, dtoPrinter.BedSize.Z);
        }
    }
}
