using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];
    private string[] allowSortValues = [nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Category),nameof(RestaurantDto.Description)];
    public GetAllRestaurantQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowPageSizes)}]");
        RuleFor(r => r.SortBy)
        .Must(value => allowSortValues.Contains(value))
        .When(q => q.SortBy != null)
        .WithMessage($"Sort by is optional, or must be in [{string.Join(".",allowSortValues)}]");
    }
}
