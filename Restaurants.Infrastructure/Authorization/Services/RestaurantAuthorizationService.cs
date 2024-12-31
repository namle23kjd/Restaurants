using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to Operation for restaurant {RestaurantName}", user.Email, resourceOperation, restaurant.Name);
        if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Read)
        {
            logger.LogInformation("Create/Read operation - successful authoriation");
            return true;
        }
        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }
        if (resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update && user.id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant owner - successful authorizaiotn");
            return true;
        }
        return false;

    }
}
