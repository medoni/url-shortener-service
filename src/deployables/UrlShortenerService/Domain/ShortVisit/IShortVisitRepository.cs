using UrlShortenerService.Domain.ShortVisit;

namespace UrlShortenerService.Domain.ShortStat;

public interface IShortVisitRepository
{
    Task AddAsync(ShortVisitEntity entity, CancellationToken c = default);
    Task<ShortVisitsStats> GetTotalVisitStatsAsync(Guid shortId, CancellationToken c = default);
}
