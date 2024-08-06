namespace TenderAPI.Response;

public class TenderListItem
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public IEnumerable<TenderSupplier> Suppliers { get; init; }
}