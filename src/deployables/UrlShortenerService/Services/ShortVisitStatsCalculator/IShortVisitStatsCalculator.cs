using UrlShortenerService.Domain.ShortVisit;
using UrlShortenerService.Services.ShortVisitClassifier;

namespace UrlShortenerService.Services.ShortVisitStatsCalculator;

public interface IShortVisitStatsCalculator
{
    ShortVisitsStats Calculate(
        Guid shortId,
        IEnumerable<ShortClassifiedVisit> visits
    );
}
