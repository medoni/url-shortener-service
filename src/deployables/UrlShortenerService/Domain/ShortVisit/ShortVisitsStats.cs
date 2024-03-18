namespace UrlShortenerService.Domain.ShortVisit;

public record ShortVisitsStats(
    Guid ShortId,
    long TotalCount
)
{
}
