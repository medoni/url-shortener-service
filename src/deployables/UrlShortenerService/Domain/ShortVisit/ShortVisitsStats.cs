using UrlShortenerService.Services.ShortVisitClassifier;

namespace UrlShortenerService.Domain.ShortVisit;

public record ShortVisitsStats(
    Guid ShortId,
    long TotalCount,
    Dictionary<(ClassifiedTypes Type, string Value), long> Items
);
