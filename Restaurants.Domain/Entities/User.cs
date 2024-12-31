using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationlity { get; set; }

    public List<Restaurant> OwnerRestaurants { get; set; } = [];
}
