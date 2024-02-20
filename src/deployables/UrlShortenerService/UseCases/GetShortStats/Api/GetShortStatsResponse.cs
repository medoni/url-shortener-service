using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.UseCases.GetShortStats.Api;

public record GetShortStatsResponse(
    Guid Id,
    long VisitTotalCount,
    long Visit24Hours
)
{
    public GetShortStatsResponse(
        ShortStatAggregate entity
    ) : this(
        entity.Id,
        entity.VisitTotalCount,
        entity.Visit24Hours
    )
    {
    }
}
