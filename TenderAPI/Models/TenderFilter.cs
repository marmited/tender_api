using TenderAPI.Enums;

namespace TenderAPI.Models;

public class TenderFilter
{
    public decimal? FromPrice { get; init; }
    public decimal? ToPrice { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int? SupplierId { get; init; }

    public SortColumn SortColumn { get; init; }
    public SortOrder SortOrder { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
} 