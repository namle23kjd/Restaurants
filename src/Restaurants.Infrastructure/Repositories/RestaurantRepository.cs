using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Contants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantRepository(RestaurantsDBContext dBContext) : IRestaurantsRepository
{
    public async Task<int> Create(Restaurant entity)
    {
        dBContext.Restaurants.Add(entity);
        await dBContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        dBContext.Remove(entity);
        await dBContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dBContext.Restaurants.ToListAsync();
        return restaurants;
    }


    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection )
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dBContext
            .Restaurants
            .Where(r => searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower)
                                              || r.Description.ToLower().Contains(searchPhraseLower)));
            var totalCount  = await baseQuery.CountAsync();

        if(sortBy != null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r  => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
            };

            var selectedColumn = columnSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
               ?  baseQuery.OrderBy(selectedColumn) 
               : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        //Pagesize = 5, pagenumber  = 2
        return (restaurants, totalCount);
    }
    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dBContext.Restaurants
            .Include(x => x.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
        return restaurant;
    }

    public Task SaveChanges() => dBContext.SaveChangesAsync();
   
}
