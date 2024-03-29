namespace UrlShortenerService.Domain.Short;

public interface IShortRepository
{
    Task AddAsync(ShortAggregate aggregate, CancellationToken c = default);
    Task<ShortAggregate> GetByIdAsync(Guid id, CancellationToken c = default);
}
