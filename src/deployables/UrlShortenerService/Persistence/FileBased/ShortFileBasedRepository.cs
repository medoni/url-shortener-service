using Microsoft.Extensions.Options;
using System.Text.Json;
using UrlShortenerService.Domain;
using UrlShortenerService.Domain.Exceptions;
using UrlShortenerService.Domain.Short;
using UrlShortenerService.Persistence.FileBased.Configuration;

namespace UrlShortenerService.Persistence.FileBased;

public class ShortFileBasedRepository : IShortRepository
{
    private readonly ILogger<ShortFileBasedRepository> _logger;
    private readonly StatsFileBasedPersistenceOptions _options;

    public ShortFileBasedRepository(
        ILogger<ShortFileBasedRepository> logger,
        IOptions<StatsFileBasedPersistenceOptions> options
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        EnsureStorageFolderExists();
    }

    public async Task AddAsync(
        ShortAggregate aggregate,
        CancellationToken c = default
    )
    {
        try
        {
            _logger.LogDebug("Adding short with '{id}' ...", aggregate.Id);

            await AddCoreAsync(aggregate, c);

            _logger.LogDebug("Successful added short with id '{id}'.", aggregate.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding short with '{id}'.", aggregate.Id);
            throw;
        }
    }

    private async Task AddCoreAsync(
        ShortAggregate aggregate,
        CancellationToken c = default
    )
    {
        var state = ((IAggregateRootWithPersistenceState<ShortAggregateState>)aggregate).Persistence;
        var file = Path.Combine(_options.Location!, $"{aggregate.Id}.json");

        try
        {
            using var fs = File.Open(file, FileMode.CreateNew);
            await JsonSerializer.SerializeAsync(fs, state, cancellationToken: c);
        }
        catch (IOException ex) when (File.Exists(file))
        {
            _logger.LogTrace(ex, "Can not adding short. The file '{file}' already exists.", file);
            throw new AggregateStillExistsException(typeof(ShortAggregate), aggregate.Id);
        }
    }

    public async Task<ShortAggregate> GetByIdAsync(
        Guid id,
        CancellationToken c = default
    )
    {
        try
        {
            _logger.LogDebug("Getting short with '{id}' ...", id);

            var result = await GetByIdCoreAsync(id, c);

            _logger.LogDebug("Successful got short with id '{id}'.", id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting short with '{id}'.", id);
            throw;
        }
    }

    private async Task<ShortAggregate> GetByIdCoreAsync(
        Guid id,
        CancellationToken c = default
    )
    {
        var file = Path.Combine(_options.Location!, $"{id}.json");
        try
        {
            using var fs = File.Open(file, FileMode.Open);
            var aggregateState = await JsonSerializer.DeserializeAsync<ShortAggregateState>(fs, cancellationToken: c);

            var aggregate = new ShortAggregate(aggregateState!);
            return aggregate;
        }
        catch (FileNotFoundException)
        {
            throw new AggregateNotFoundException(typeof(ShortAggregate), id);
        }
    }

    private void EnsureStorageFolderExists()
    {
        if (!Directory.Exists(_options.Location))
        {
            Directory.CreateDirectory(_options.Location!);
        }
    }
}
