using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.UseCases.GetShortStats.Api;

public record GetShortStatsResponse(
    Guid Id,
    long VisitTotalCount,
    long Visit24Hours
)
{
    public GetShortStatsResponse(
        ShortVisitEntity entity
    ) : this(
        entity.Id,
        0,
        0
    )
    {
    }
}
