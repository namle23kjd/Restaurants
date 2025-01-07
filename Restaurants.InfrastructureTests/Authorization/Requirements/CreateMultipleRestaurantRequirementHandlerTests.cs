using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class CreateMultipleRestaurantRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurant_ShouldSucced()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId =  currentUser.Id,
                },
                new()
                {
                    OwnerId =  currentUser.Id,
                },
                                new()
                {
                    OwnerId =  "2",
                },
            };

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreateMultipleRestaurantRequirement(2);
            var handler = new CreateMultipleRestaurantRequirementHandler(restaurantRepositoryMock.Object, userContextMock.Object);


            var context = new AuthorizationHandlerContext([requirement], null, null);

            await handler.HandleAsync(context);

            // assert

            context.HasSucceeded.Should().BeTrue();
        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurant_ShouldFail()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId =  currentUser.Id,
                },
                new()
                {
                    OwnerId =  "2",
                },
            };

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(restaurants);

            var requirement = new CreateMultipleRestaurantRequirement(2);
            var handler = new CreateMultipleRestaurantRequirementHandler(restaurantRepositoryMock.Object, userContextMock.Object);


            var context = new AuthorizationHandlerContext([requirement], null, null);

            await handler.HandleAsync(context);

            // assert

            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();
        }
    }
}