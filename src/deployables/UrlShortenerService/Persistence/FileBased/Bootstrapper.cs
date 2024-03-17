using UrlShortenerService.Domain.Short;
using UrlShortenerService.Persistence.Bootstrap;
using UrlShortenerService.Persistence.FileBased.Configuration;

namespace UrlShortenerService.Persistence.FileBased;

public static class Bootstrapper
{
    public static IShortsPersistenceOptionsBuilder AddFileBased(
        this IShortsPersistenceOptionsBuilder builder,
        Action<StatsFileBasedPersistenceOptions> options
    )
    {
        builder.Services.Configure<StatsFileBasedPersistenceOptions>(options);
        builder.Services.AddScoped<IShortRepository, ShortFileBasedRepository>();

        return builder;
    }
}
