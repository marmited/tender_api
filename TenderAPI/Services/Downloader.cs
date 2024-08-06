using System.Text.Json;
using TenderAPI.Models;

namespace TenderAPI.Services;

public class Downloader : IDownloader
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<Downloader> _logger;
    private const string BaseUrl = "https://tenders.guru/api/pl/tenders/?page=";

    public Downloader(IHttpClientFactory httpClientFactory, ILogger<Downloader> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    public async Task<TenderApiBasicResponseRoot> GetTendersAsync(int pageNumber, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClientFactory.CreateClient()
                .GetAsync($"{BaseUrl}{pageNumber}", cancellationToken);

            var result = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<TenderApiBasicResponseRoot>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error has occured during requesting external API.");
            throw;
        }
    }
}