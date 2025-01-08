using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Contants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUserTest_WithAuthenticaUser_ShouldReturnCurrentUser()
        {

            // arange

            var dateOfBirth = new DateOnly(1990, 01, 01);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier,"1"),
                new(ClaimTypes.Email,"test@test.com"),
                new(ClaimTypes.Role,UserRoles.Admin),
                new(ClaimTypes.Role,UserRoles.User),
                new("Nationality","Indian"),
                new("DateOfBrith",dateOfBirth.ToString("yyyy-MM-dd"))
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"Test"));
            httpContextAccessor.Setup(h => h.HttpContext).Returns(new
                 DefaultHttpContext()
            {
                User = user
            });

            var userContext = new UserContext(httpContextAccessor.Object);

            //act
            var currentUser = userContext.GetCurrentUser();


            //asset
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.Nationality.Should().Be("Indian");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
        }

        [Fact()]
        public void GetCurrentUserTest_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            //Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null);
            var userContext = new UserContext(httpContextAccessorMock.Object);
            //act
            Action action = () => userContext.GetCurrentUser();

            //assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User context is not present");
        }
    }
}