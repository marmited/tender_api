using System.Text.Json.Serialization;

namespace TenderAPI.Models;
public class TenderApiBasicDataAwardedSupplier
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}
