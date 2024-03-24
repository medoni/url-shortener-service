namespace UrlShortenerService.Services.ShortVisitStatsCalculator;

public static class ShortVisitStatsCalculatorServiceCollectionExtensions
{
    public static IServiceCollection AddShortVisitStatsCalculator(
        this IServiceCollection services
    )
    {
        services.AddTransient<IShortVisitStatsCalculator, DefaultShortVisitStatsCalculator>();
        return services;
    }
}
