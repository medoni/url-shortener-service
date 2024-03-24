using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Domain.ShortVisit;

namespace UrlShortenerService.Persistence.NullBased;

public class ShortStatNullBasedRepository : IShortVisitRepository
{
    public Task AddAsync(ShortVisitEntity aggregate, CancellationToken c = default)
    {
        throw new NotImplementedException();
    }

    public Task<ShortVisitsStats> GetTotalVisitStatsAsync(Guid shortId, CancellationToken c = default)
    {
        throw new NotImplementedException();
    }
}
