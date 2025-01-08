using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middlerwares.Tests
{
    public class ErrorHandlingMiddleTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShiuldCallNextDelegate()
        {
            // arrage
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);  
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();

            await middleware.InvokeAsync(context, nextDelegateMock.Object);

            //assert
            nextDelegateMock.Verify(next => next(context), Times.Once);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var notFoundException = new NotFoundException(nameof(Restaurant), "1");

            //act 
            await middleware.InvokeAsync(context, _ => throw notFoundException);

            //Assert
            context.Response.StatusCode.Should().Be(404);   
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode403()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new ForbidException();

            //act 
            await middleware.InvokeAsync(context, _ => throw exception);

            //Assert
            context.Response.StatusCode.Should().Be(403);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode500()
        {
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddle>>();
            var middleware = new ErrorHandlingMiddle(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new Exception();

            //act 
            await middleware.InvokeAsync(context, _ => throw exception);

            //Assert
            context.Response.StatusCode.Should().Be(500);
        }
    }
}