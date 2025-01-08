using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;


public static class ServicesCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection String: {connectionString}");
        services.AddDbContext<RestaurantsDBContext>(options => options.UseSqlServer(connectionString)
        .EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDBContext>();

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddAuthorizationBuilder()
            .AddPolicy("HasNationality", builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", " Polish "))
            .AddPolicy(PolicyNames.AtLeast20, builder => builder.AddRequirements(new MinimumAgeRequirements(20)))
            .AddPolicy(PolicyNames.CreateAtLeast2Restaurant,
                   builder => builder.AddRequirements(new CreateMultipleRestaurantRequirement(2)));
            ;

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementsHandler>();
        services.AddScoped<IAuthorizationHandler, CreateMultipleRestaurantRequirementHandler>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
    }
}