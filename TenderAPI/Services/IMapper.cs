using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public interface IMapper
{
    List<TenderListItem> Map(TenderApiBasicResponseRoot[] source);
}