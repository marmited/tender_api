using TenderAPI.Models;

namespace TenderAPI.Services;

public interface IDownloader
{
    Task<TenderApiBasicResponseRoot> GetTendersAsync(int pageNumber, CancellationToken cancellationToken);
}