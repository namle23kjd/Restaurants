using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Command.UpdateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Command.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        //arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();

        var restaurant = new Restaurant();
        var command = new CreateRestaurantCommand();
        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();

        restaurantRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>())).ReturnsAsync(1);
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@test.com", [], null,null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object, restaurantRepositoryMock.Object, userContextMock.Object);
        //act
        var  result = await commandHandler.Handle(command, CancellationToken.None);
        //assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantRepositoryMock.Verify(repo => repo.Create(restaurant), Times.Once);
    }

}