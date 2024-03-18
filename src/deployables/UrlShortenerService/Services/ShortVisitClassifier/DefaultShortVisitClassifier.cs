using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.Services.ShortVisitClassifier;

public class DefaultShortVisitClassifier : IShortVisitClassifier
{
    public ShortClassifiedVisit Classify(ShortVisitEntity visit)
    {
        return new ShortClassifiedVisit();
    }
}
