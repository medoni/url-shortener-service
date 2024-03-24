namespace UrlShortenerService.Services.ShortVisitClassifier;

public record ClassifiedItem(
    ClassifiedTypes Type,
    string Value
);
