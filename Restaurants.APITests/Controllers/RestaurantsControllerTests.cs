using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;


namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200OK()
    {
        // arrage
        var clinet = _factory.CreateClient();

        //act
        var result = await clinet.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns400BadRequest()
    {
        // arrage
        var clinet = _factory.CreateClient();

        //act
        var result = await clinet.GetAsync("/api/restaurants");

        //assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}