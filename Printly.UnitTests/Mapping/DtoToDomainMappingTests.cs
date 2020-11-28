using AutoMapper;
using MongoDB.Bson;
using Printly.Mapping;
using System;
using Xunit;

namespace Printly.UnitTests.Mapping
{
    public class DtoToDomainMappingTests
    {
        [Fact]
        public void GivenPrinterDto_WhenMappedToPrinterDomain_ThenMappingCorrect()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoToDomainMapping>();
            });
            var sut = new Mapper(config);

            var dtoPrinter = new Dto.Request.Printer()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MarlinId = Guid.NewGuid().ToString(),
                Name = "Hello World",
                BedSize = new Dto.Request.Dimensions()
                {
                    X = 100,
                    Y = 200,
                    Z = 300
                }
            };

            // Act
            var domainPrinter = sut.Map<Domain.Models.Printer>(dtoPrinter);

            // Assert
            Assert.Equal(dtoPrinter.Id, domainPrinter.Id.ToString());
            Assert.Equal(dtoPrinter.MarlinId, domainPrinter.MarlinId);
            Assert.Equal(dtoPrinter.Name, domainPrinter.Name);
            Assert.Equal(dtoPrinter.BedSize.X, domainPrinter.BedSizeX);
            Assert.Equal(dtoPrinter.BedSize.Y, domainPrinter.BedSizeY);
            Assert.Equal(dtoPrinter.BedSize.Z, domainPrinter.BedSizeZ);
        }
    }
}
