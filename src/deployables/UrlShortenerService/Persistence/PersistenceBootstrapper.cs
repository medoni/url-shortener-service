using UrlShortenerService.Persistence.Bootstrap;

namespace UrlShortenerService.Persistence;

public static class PersistenceBootstrapper
{
    public static IServiceCollection AddShortsPersistence(
        this IServiceCollection services,
        Action<IShortsPersistenceOptionsBuilder> optionsBuilder
    )
    {
        var shortOptionsBuilder = new ShortsPersistenceOptionsBuilder(
            services
        );

        optionsBuilder(shortOptionsBuilder);

        return services;
    }

    public static IServiceCollection AddShortStatsPersistence(
        this IServiceCollection services,
        Action<IShortVisitsPersistenceOptionsBuilder> optionsBuilder
    )
    {
        var statOptionsBuilder = new ShortStatsPersistenceOptionsBuilder(
            services
        );

        optionsBuilder(statOptionsBuilder);

        return services;
    }

    private class ShortsPersistenceOptionsBuilder : IShortsPersistenceOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        public ShortsPersistenceOptionsBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }

    private class ShortStatsPersistenceOptionsBuilder : IShortVisitsPersistenceOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        public ShortStatsPersistenceOptionsBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}
