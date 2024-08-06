using TenderAPI.Enums;
using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public class DataProvider : IDataProvider
{
    private readonly ICacheManager _memoryCache;
    private readonly IDownloader _downloader;
    private readonly IMapper _mapper;
    private const string CacheKey = "tenders_key";

    public DataProvider (
         ICacheManager memoryCache,
         IDownloader downloader,
         IMapper mapper
        )
    {
        _memoryCache = memoryCache;
        _downloader = downloader;
        _mapper = mapper;
    }
    
    public async Task<TenderWrapper> GetTendersAsync(TenderFilter filter, CancellationToken cancellationToken)
    {
        var data = await GetDataAsync(cancellationToken);

        data = Filter(data, filter).ToList();
        var totalCount = data.Count();
        data = Order(data, filter).ToList();
        
        return new TenderWrapper()
        {
            TotalCount = totalCount,
            Items = data,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            PageCount = data.Count()
        };
    }
    
    public async Task<TenderListItem> GetTenderAsync(int id, CancellationToken cancellationToken)
    {
        var data = await GetDataAsync(cancellationToken);

        var item = data.FirstOrDefault(x => x.Id == id);
        
        return item;
    }

    private async Task<IEnumerable<TenderListItem>> GetDataAsync(CancellationToken cancellationToken)
    {
        (bool success, var data) = _memoryCache.GetData<IEnumerable<TenderListItem>>(CacheKey);

        if (!success)
        {
            data = await DownloadDataAsync(cancellationToken);
            _memoryCache.SetData(CacheKey, data);
        }

        return data;
    }


    private async Task<List<TenderListItem>> DownloadDataAsync(CancellationToken cancellationToken)
    {
        var tasks = new List<Task<TenderApiBasicResponseRoot>>();
            
        for (int i = 1; i <= 100; i++)
        {
            tasks.Add(_downloader.GetTendersAsync(i, cancellationToken));
        }
            
        var awaitedTasks = await Task.WhenAll(tasks);

        var convertedResult = _mapper.Map(awaitedTasks);
        return convertedResult;
    }

    private IEnumerable<TenderListItem> Filter(IEnumerable<TenderListItem> listItems, TenderFilter filter)
    {
        if (filter.FromDate != null)
            listItems = listItems.Where(x => x.Date >= filter.FromDate);
        
        if (filter.ToDate != null)
            listItems = listItems.Where(x => x.Date <= filter.ToDate);

        if (filter.FromPrice != null)
            listItems = listItems.Where(x => x.Price >= filter.FromPrice);
        
        if (filter.ToPrice != null)
            listItems = listItems.Where(x => x.Price <= filter.ToPrice);
        
        if (filter.SupplierId != null)
            listItems = listItems.Where(x => x.Suppliers.Any(q=> q.Id == filter.SupplierId));

        return listItems;
    }
    
    private IEnumerable<TenderListItem> Order(IEnumerable<TenderListItem> listItems, TenderFilter filter)
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