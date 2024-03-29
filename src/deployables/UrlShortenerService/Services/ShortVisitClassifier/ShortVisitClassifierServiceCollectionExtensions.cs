namespace UrlShortenerService.Services.ShortVisitClassifier;

public static class ShortVisitClassifierServiceCollectionExtensions
{
    public static IServiceCollection AddShortVisitClassifier(
        this IServiceCollection services
    )
    {
        services.AddTransient<IShortVisitClassifier, DefaultShortVisitClassifier>();
        return services;
    }
}
