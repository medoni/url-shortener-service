using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Persistence.Bootstrap;

namespace UrlShortenerService.Persistence.NullBased;

public static class Bootstrapper
{
    public static IShortStatsPersistenceOptionsBuilder AddNullBased(
        this IShortStatsPersistenceOptionsBuilder builder
    )
    {
        builder.Services.AddScoped<IShortStatRepository, ShortStatNullBasedRepository>();

        return builder;
    }
}
