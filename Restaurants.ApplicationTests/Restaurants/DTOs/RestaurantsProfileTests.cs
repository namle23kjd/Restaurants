using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Command.CreateRestaurant;
using Restaurants.Application.Restaurants.Command.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.DTOs.Tests;

public class RestaurantsProfileTests
{
    private IMapper _mapper;
    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDTO_MapCorrectly()
    {
        // arrange

        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            Address = new Address()
            {
                City = "Test City",
                PostalCode = "12-345",
                Street = "Test Street"
            }
        };

        //act

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // arresst
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);


    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantToRestaurantDTO_MapCorrectly()
    {
        // arrange

        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            City = "Test City",
            PostalCode = "12-345",
            Street = "Test Street"
        };

        //act

        var restaurant = _mapper.Map<Restaurant>(command);

        // arresst
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantToRestaurantDTO_MapCorrectly()
    {
        // arrange

        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Update Restaurant",
            Description = "Update Description",
            HasDelivery  = false
        };

        //act

        var restaurant = _mapper.Map<Restaurant>(command);

        // arresst
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }
}