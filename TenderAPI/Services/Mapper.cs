using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using TenderAPI.Enums;
using TenderAPI.Models;
using TenderAPI.Response;

namespace TenderAPI.Services;

public class Mapper : IMapper
{
    private readonly ILogger<Mapper> _mapper;

    public Mapper(ILogger<Mapper> mapper)
    {
        _mapper = mapper;
    }
    public List<TenderListItem> Map(TenderApiBasicResponseRoot[] awaitedTasks)
    {
        try
        {
            var convertedResult = awaitedTasks.SelectMany(x => x.Data).Select(q => new TenderListItem()
            {
                Id = Int32.Parse(q.Id),
                Date = DateTime.Parse(q.Date),
                Description = q.Description,
                Price = Decimal.Parse(q.AwardedValueEur, CultureInfo.GetCultureInfo("en-US")),
                Suppliers = q.Awarded.SelectMany(y => y.Suppliers).Select(z => new TenderSupplier()
                {
                    Id = z.Id,
                    Name = z.Name
                }),
                Title = q.Title
            }).ToList();

            return convertedResult;

        }
        catch (Exception)
        {
            _mapper.LogError("An error has occured during mapping.");
            throw;
        }
    }
}