using MediatR;

namespace Restaurants.Application.Dishes.Command.DeleteDishes;

public class DeleteDishesForRestaurantCommand(int resstaurantId) : IRequest
{
    public int RestaurantId { get;} = resstaurantId;
}
