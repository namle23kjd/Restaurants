namespace Restaurants.Application.Common;

public class PagedResult<T>
{
    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ItemsFrom = pageSize *  (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
        //pageSize  = 5, pageNumber = 2
        //skip : pageSie * (pageNumber - 1) => 5
        //itemFrom: 5 + 1 => 6
        //itemTo : 6 + 5 - 1 => 10
    }
    public IEnumerable<T>  Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}
