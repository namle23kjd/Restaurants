using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Services;
using Xunit;


namespace Restaurants.Application.Restaurants.Command.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _repositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _repositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        //arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Old Name",
            Description = "Old Description"
        };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(service => service.Authorize(restaurant, ResourceOperation.Update)).Returns(true);

        //act
        await _handler.Handle(command, CancellationToken.None);

        //asert
        _repositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }

    

    [Fact()]
    public async Task Handle_WithUnauthorizationUser_SHouldThrowForBidException()
    {
        //Arrange
        var restaurantId = 2;
        var request = new UpdateRestaurantCommand
        {
            Id = restaurantId
        };

        var existingRestaurant = new Restaurant()
        {
            Id = restaurantId
        };
        _repositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId)).ReturnsAsync(existingRestaurant);

        _restaurantAuthorizationServiceMock.Setup(service => service.Authorize(existingRestaurant, ResourceOperation.Update)).Returns(false);

        //act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        //Assert
        await act.Should().ThrowAsync<ForbidException>();
    }
}