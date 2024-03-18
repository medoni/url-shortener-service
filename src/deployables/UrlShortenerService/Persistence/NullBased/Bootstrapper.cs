using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Persistence.Bootstrap;

namespace UrlShortenerService.Persistence.NullBased;

public static class Bootstrapper
{
    public static IShortVisitsPersistenceOptionsBuilder AddNullBased(
        this IShortVisitsPersistenceOptionsBuilder builder
    )
    {
        builder.Services.AddScoped<IShortVisitRepository, ShortStatNullBasedRepository>();

        return builder;
    }
}
