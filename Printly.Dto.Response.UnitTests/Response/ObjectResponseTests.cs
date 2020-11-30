﻿using Printly.Dto.Response;
using System.Net;
using Xunit;

namespace Printly.Dto.UnitTests.Response
{
    public class ObjectResponseTests
    {
        [Fact]
        public void GivenCode_AndMessage_WhenConstructed_ThenInstanceErrorCorrect()
        {
            // Arrange
            var code = HttpStatusCode.NotFound;
            var message = "Hello World!";

            // Act
            var sut = new ObjectResponse<object>(
                code,
                message);

            // Assert
            Assert.NotNull(sut.Error);
            Assert.Equal(code, sut.Error.HttpStatusCode);
            Assert.Equal(message, sut.Error.Message);
        }
    }
}
