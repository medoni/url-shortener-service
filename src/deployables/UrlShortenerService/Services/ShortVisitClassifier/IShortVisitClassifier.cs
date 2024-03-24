using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.Services.ShortVisitClassifier;

public interface IShortVisitClassifier
{
    ShortClassifiedVisit Classify(ShortVisitEntity visit);
}
