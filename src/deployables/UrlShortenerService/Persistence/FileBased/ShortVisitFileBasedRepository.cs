using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Text.Json;
using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Domain.ShortVisit;
using UrlShortenerService.Persistence.FileBased.Configuration;
using UrlShortenerService.Services.ShortVisitClassifier;
using UrlShortenerService.Services.ShortVisitStatsCalculator;

namespace UrlShortenerService.Persistence.FileBased;

public class ShortVisitFileBasedRepository : IShortVisitRepository
{
    private readonly ILogger<ShortFileBasedRepository> _logger;
    private readonly ShortVisitFileBasedPersistenceOptions _options;
    private readonly IShortVisitStatsCalculator _statsCalculator;
    private readonly IShortVisitClassifier _visitClassifier;

    public ShortVisitFileBasedRepository(
        ILogger<ShortFileBasedRepository> logger,
        IOptions<ShortVisitFileBasedPersistenceOptions> options,
        IShortVisitStatsCalculator statsCalculator,
        IShortVisitClassifier visitClassifier
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _statsCalculator = statsCalculator ?? throw new ArgumentNullException(nameof(statsCalculator));
        _visitClassifier = visitClassifier ?? throw new ArgumentNullException(nameof(visitClassifier));

        EnsureStorageFolderExists();
    }

    public async Task AddAsync(
        ShortVisitEntity entity,
        CancellationToken c = default
    )
    {
        try
        {
            _logger.LogDebug("Adding short visit with short id '{shortId}' ...", entity.ShortId);

            await AddCoreAsync(entity, c);

            _logger.LogDebug("Successful added short visit with short id '{shortId}'.", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding short visit with short '{shortId}'.", entity.Id);
            throw;
        }
    }

    private async Task AddCoreAsync(
        ShortVisitEntity entity,
        CancellationToken c = default
    )
    {
        var tryCount = 1;
        string file = null!;

        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<IOException>(_ => File.Exists(file)),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            Delay = TimeSpan.FromMilliseconds(1),
            OnRetry = _ => { ++tryCount; return ValueTask.CompletedTask; }
        };

        var resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .AddTimeout(TimeSpan.FromSeconds(10))
            .Build();

        await resiliencePipeline.ExecuteAsync(async _ =>
        {
            file = Path.Combine(_options.Location!, $"{entity.ShortId}-{entity.VisitedAt.ToUnixTimeSeconds()}-{tryCount}.json");
            await AddCoreAsync(entity, file, c);
        });
    }

    private async Task AddCoreAsync(
        ShortVisitEntity entity,
        string file,
        CancellationToken c = default
    )
    {
        using var fs = File.Open(file, FileMode.CreateNew);
        await JsonSerializer.SerializeAsync(fs, entity, cancellationToken: c);
    }

    public async Task<ShortVisitsStats> GetTotalVisitStatsAsync(
        Guid shortId,
        CancellationToken c = default
    )
    {
        try
        {
            _logger.LogDebug("Getting total visit count for short with id '{shortId}' ...", shortId);

            var totalCount = await GetTotalVisitStatsCoreAsync(shortId, c);

            _logger.LogDebug("Got {totalCount} total visit count for short with id '{shortId}'.", totalCount, shortId);

            return totalCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total visit count for short with id '{shortId}'.", shortId);
            throw;
        }
    }

    private async Task<ShortVisitsStats> GetTotalVisitStatsCoreAsync(
        Guid shortId,
        CancellationToken c = default
    )
    {
        var pattern = $"{shortId}-*.json";
        var files = Directory.GetFiles(_options.Location!, pattern);
        var classifiedVisits = await files
            .ToAsyncEnumerable()
            .SelectAwait(async file => await ReadVisitAsync(file))
            .Select(visit => _visitClassifier.Classify(visit))
            .ToArrayAsync();

        var calculatedStats = _statsCalculator.Calculate(shortId, classifiedVisits);
        return calculatedStats;
    }

    private async Task<ShortVisitEntity> ReadVisitAsync(string file)
    {
        using var fs = File.Open(file, FileMode.Open);
        var entity = await JsonSerializer.DeserializeAsync<ShortVisitEntity>(fs);
        return entity!;
    }

    private void EnsureStorageFolderExists()
    {
        if (!Directory.Exists(_options.Location))
        {
            Directory.CreateDirectory(_options.Location!);
        }
    }
}
