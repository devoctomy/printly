using Printly.Exceptions;
using System;
using Xunit;

namespace Printly.UnitTests.Exceptions
{
    public class PrintlyExceptionTests
    {
        [Fact]
        public void GivenMessage_WhenConstuct_ThenMessageSet()
        {
            // Arrange
            var message = "Hello World!";

            // Act
            var sut = new PrintlyException(message);

            // Assert
            Assert.Equal(message, sut.Message);
        }

        [Fact]
        public void GivenMessage_AndInnerException_WhenConstuct_ThenMessageSet()
        {
            // Arrange
            var message = "Hello World!";
            var innerException = new Exception("Stuff");

            // Act
            var sut = new PrintlyException(
                message,
                innerException);

            // Assert
            Assert.Equal(message, sut.Message);
            Assert.Equal(innerException, sut.InnerException);
        }
    }
}
