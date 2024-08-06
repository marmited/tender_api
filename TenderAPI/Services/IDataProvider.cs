using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public interface IDataProvider
{
    Task<TenderWrapper> GetTendersAsync(TenderFilter filter, CancellationToken cancellationToken);
}