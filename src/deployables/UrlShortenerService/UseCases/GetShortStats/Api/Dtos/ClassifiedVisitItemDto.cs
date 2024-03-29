using UrlShortenerService.Services.ShortVisitClassifier;

namespace UrlShortenerService.UseCases.GetShortStats.Api.Dtos;

public record ClassifiedVisitItemDto(
    ClassifiedTypes Type,
    string Value,
    long Count
);
