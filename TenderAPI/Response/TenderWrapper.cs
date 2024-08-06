namespace TenderAPI.Response;

public class TenderWrapper
{
    public IEnumerable<TenderListItem> Items { get; init; }
    public int TotalCount { get; init; }
    public int PageCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}