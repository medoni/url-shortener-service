namespace UrlShortenerService.Domain.ShortStat;

public record ShortVisitEntity(
    Guid Id,
    Guid ShortId,
    string redirectTo,
    DateTimeOffset VisitedAt,
    string? Referrer,
    string? UserAgent
)
{
}