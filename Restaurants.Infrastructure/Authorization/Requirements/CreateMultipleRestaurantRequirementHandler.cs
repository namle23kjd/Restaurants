using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreateMultipleRestaurantRequirementHandler(IRestaurantsRepository restaurantsRepository, IUserContext userContext) : AuthorizationHandler<CreateMultipleRestaurantRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateMultipleRestaurantRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        var restaurants =  await restaurantsRepository.GetAllAsync();
        var userRestaurantsCreate = restaurants.Count(r => r.OwnerId == currentUser!.id);
        if(userRestaurantsCreate >= requirement.MinimumRestaurantCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
