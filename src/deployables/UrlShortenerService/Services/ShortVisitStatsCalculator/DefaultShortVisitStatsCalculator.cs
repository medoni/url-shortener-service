using UrlShortenerService.Domain.ShortVisit;
using UrlShortenerService.Services.ShortVisitClassifier;

namespace UrlShortenerService.Services.ShortVisitStatsCalculator;

public class DefaultShortVisitStatsCalculator : IShortVisitStatsCalculator
{
    public ShortVisitsStats Calculate(
        Guid shortId,
        IEnumerable<ShortClassifiedVisit> visits
    )
    {
        var classifiedItems = visits
            .SelectMany(x => x.Items)
            .GroupBy(x => (Type: x.Type, Value: x.Value))
            .ToDictionary(
                k => k.Key,
                v => v.LongCount()
            );

        return new ShortVisitsStats(
            shortId,
            visits.LongCount(),
            classifiedItems
        );
    }
}
