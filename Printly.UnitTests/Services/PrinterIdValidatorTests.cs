using MongoDB.Bson;
using Printly.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class PrinterIdValidatorTests
    {
        [Fact]
        public void GivenInvalidId_WhenValidate_ThenErrorReturned()
        {
            // Arrange
            var sut = new PrinterIdValidator();

            // Act
            var result = sut.Validate("Bob Hoskins");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        }

        [Fact]
        public void GivenWhitespaceId_WhenValidate_ThenErrorReturned()
        {
            // Arrange
            var sut = new PrinterIdValidator();

            // Act
            var result = sut.Validate(" ");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        }

        [Fact]
        public void GivenValidId_WhenValidate_ThenNullReturned()
        {
            // Arrange
            var sut = new PrinterIdValidator();

            // Act
            var result = sut.Validate(ObjectId.GenerateNewId().ToString());

            // Assert
            Assert.Null(result);
        }
    }
}
