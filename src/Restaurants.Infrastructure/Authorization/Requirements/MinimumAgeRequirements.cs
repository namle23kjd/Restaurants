using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirements(int minimumage) : IAuthorizationRequirement
{
    public int MinimumAge { get; set; } = minimumage;

}
