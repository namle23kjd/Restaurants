using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Command.Queries.GetDishByIdForRestaurant;

public class GetDishByIdRestaurantQuery(int restaurantId, int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; set; } = restaurantId;
    public int DishId { get; set; } = dishId;
}
