using System.Text.Json.Serialization;

namespace TenderAPI.Models;
    public class TenderApiBasicResponseRoot
    {
        [JsonPropertyName("page_count")]
        public int PageCount { get; set; }

        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("data")]
        public List<TenderApiBasicData> Data { get; set; }
    }