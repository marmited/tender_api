using TenderAPI.Enums;
using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public class TenderFilteringService : ITenderFilteringService
{
    public IEnumerable<TenderListItem> Filter(IEnumerable<TenderListItem> listItems, TenderFilter filter)
    {
        if (filter.FromDate != null)
            listItems = listItems.Where(x => x.Date >= filter.FromDate.Value.Date);
        
        if (filter.ToDate != null)
            listItems = listItems.Where(x => x.Date <= filter.ToDate.Value.Date);

        if (filter.FromPrice != null)
            listItems = listItems.Where(x => x.Price >= filter.FromPrice);
        
        if (filter.ToPrice != null)
            listItems = listItems.Where(x => x.Price <= filter.ToPrice);
        
        if (filter.SupplierId != null)
            listItems = listItems.Where(x => x.Suppliers.Any(q=> q.Id == filter.SupplierId));

        return listItems;
    }
    
    public IEnumerable<TenderListItem> Order(IEnumerable<TenderListItem> listItems, TenderFilter filter)
    {
        listItems = filter.SortColumn switch
        {
            SortColumn.Date when filter.SortOrder == SortOrder.Dsc => listItems.OrderByDescending(x => x.Date),
            SortColumn.Date when filter.SortOrder == SortOrder.Asc => listItems.OrderBy(x => x.Date),
            SortColumn.Price when filter.SortOrder == SortOrder.Dsc => listItems.OrderByDescending(x => x.Price),
            SortColumn.Price when filter.SortOrder == SortOrder.Asc => listItems.OrderBy(x => x.Price),
            _ => listItems.OrderBy(x=>x.Id)
        };
        
        listItems = listItems
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize);

        return listItems;
    }
}