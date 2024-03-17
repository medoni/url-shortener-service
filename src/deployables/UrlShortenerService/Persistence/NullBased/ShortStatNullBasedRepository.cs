using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.Persistence.NullBased;

public class ShortStatNullBasedRepository : IShortStatRepository
{
    public Task AddAsync(ShortStatAggregate aggregate, CancellationToken c = default)
    {
        throw new NotImplementedException();
    }

    public Task<ShortStatAggregate> GetByIdAsync(Guid id, CancellationToken c = default)
    {
        throw new NotImplementedException();
    }
}
