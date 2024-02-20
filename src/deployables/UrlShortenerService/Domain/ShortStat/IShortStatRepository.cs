namespace UrlShortenerService.Domain.ShortStat;

public interface IShortStatRepository
{
    Task AddAsync(ShortStatAggregate aggregate, CancellationToken c = default);
    Task<ShortStatAggregate> GetByIdAsync(Guid id, CancellationToken c = default);
}
