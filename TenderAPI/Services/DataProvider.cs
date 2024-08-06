using TenderAPI.Enums;
using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public class DataProvider : IDataProvider
{
    private readonly ICacheManager _memoryCache;
    private readonly IDownloader _downloader;
    private readonly IMapper _mapper;
    private readonly ITenderFilteringService _filteringService;
    private const string CacheKey = "tenders_key";

    public DataProvider (
         ICacheManager memoryCache,
         IDownloader downloader,
         IMapper mapper,
         ITenderFilteringService filteringService
        )
    {
        _memoryCache = memoryCache;
        _downloader = downloader;
        _mapper = mapper;
        _filteringService = filteringService;
    }
    
    public async Task<TenderWrapper> GetTendersAsync(TenderFilter filter, CancellationToken cancellationToken)
    {
        var data = await GetDataAsync(cancellationToken);

        data = _filteringService.Filter(data, filter).ToList();
        var totalCount = data.Count();
        data = _filteringService.Order(data, filter).ToList();
        
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
}