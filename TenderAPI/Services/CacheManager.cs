using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public class CacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOptions<CacheOptions> _cacheOptions;
    private readonly ILogger<CacheManager> _logger;

    public CacheManager (
         IMemoryCache memoryCache,
         IOptions<CacheOptions> cacheOptions,
         ILogger<CacheManager> logger)
    {
        _memoryCache = memoryCache;
        _cacheOptions = cacheOptions;
        _logger = logger;
    }

    public (bool success, T data) GetData<T>(string key) where T: class
    {
        bool success = _memoryCache.TryGetValue<IEnumerable<TenderListItem>>(key, out var data);
        
        try
        {
            if (data != null)
            {
                T result = (T)data;
                return (success, result);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Cached object cannot be parsed to class {typeof(T)}");
            throw;
        }
        
        return (success, null);
    }

    public void SetData(string key, object data)
    {
        _memoryCache.Set(key, data, DateTimeOffset.Now.AddSeconds(_cacheOptions.Value.AbsoluteTimeInSeconds));
    }
}