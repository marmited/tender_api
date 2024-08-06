using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public interface ITenderFilteringService
{
    IEnumerable<TenderListItem> Filter(IEnumerable<TenderListItem> listItems, TenderFilter filter);
    IEnumerable<TenderListItem> Order(IEnumerable<TenderListItem> listItems, TenderFilter filter);
}