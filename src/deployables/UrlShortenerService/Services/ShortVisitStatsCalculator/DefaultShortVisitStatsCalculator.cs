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
        return new ShortVisitsStats(
            shortId,
            visits.LongCount()
        );
    }
}
