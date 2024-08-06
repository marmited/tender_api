using System.Text.Json.Serialization;

namespace TenderAPI.Models;

public class TenderApiBasicData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("awarded_value_eur")]
    public string AwardedValueEur { get; set; }

    [JsonPropertyName("awarded")]
    public List<TenderApiBasicDataAwarded> Awarded { get; set; }
}