namespace UrlShortenerService.Services.ShortVisitClassifier;

public record ShortClassifiedVisit(
    IReadOnlyList<ClassifiedItem> Items
);
