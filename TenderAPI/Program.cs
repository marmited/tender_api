using TenderAPI;
using TenderAPI.Models;
using TenderAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDownloader, Downloader>();
builder.Services.AddScoped<IDataProvider, DataProvider>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<ICacheManager, CacheManager>();

builder.Services.AddHttpClient<Downloader>()
    .ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler() { MaxConnectionsPerServer = 100 });

builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("CacheOptions"));

//builder.Services.AddHttpClient<IDataProvider, DataProvider>();
builder.Services.AddMemoryCache();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/tender", async (IDataProvider dataProvider, CancellationToken cancellationToken, [AsParameters]TenderFilter filter) =>
    {
        var tenders = await dataProvider.GetTendersAsync(filter, cancellationToken);
        return tenders;
    })
    .WithName("GetTender")
    .WithOpenApi();

app.Run();