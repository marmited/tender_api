using System.Text.Json.Serialization;

namespace TenderAPI.Models;

public class TenderApiBasicDataAwarded
{
    [JsonPropertyName("suppliers")]
    public List<TenderApiBasicDataAwardedSupplier> Suppliers { get; set; }
}