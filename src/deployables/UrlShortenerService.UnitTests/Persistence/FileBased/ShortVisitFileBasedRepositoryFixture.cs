using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;
using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Domain.ShortVisit;
using UrlShortenerService.Persistence.FileBased;
using UrlShortenerService.Persistence.FileBased.Configuration;
using UrlShortenerService.Services.ShortVisitClassifier;
using UrlShortenerService.Services.ShortVisitStatsCalculator;

namespace UrlShortenerService.UnitTests.Persistence.FileBased;

[TestFixture]
[Category("Unit")]
public class ShortVisitFileBasedRepositoryTests
{
    private ShortVisitFileBasedRepository Sut { get; set; }

    private Mock<ILogger<ShortFileBasedRepository>> LoggerMock { get; set; }
    private ShortVisitFileBasedPersistenceOptions PersistenceOptions { get; set; }
    private Mock<IShortVisitStatsCalculator> StatsCalculator { get; set; }
    private Mock<IShortVisitClassifier> VisitClassifierMock { get; set; }

    [SetUp]
    public void SetUp()
    {
        LoggerMock = new Mock<ILogger<ShortFileBasedRepository>>();
        PersistenceOptions = new ShortVisitFileBasedPersistenceOptions();
        StatsCalculator = new Mock<IShortVisitStatsCalculator>();
        VisitClassifierMock = new Mock<IShortVisitClassifier>();

        PersistenceOptions.Location = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Sut = new ShortVisitFileBasedRepository(
            LoggerMock.Object,
            Options.Create(PersistenceOptions),
            StatsCalculator.Object,
            VisitClassifierMock.Object
        );
    }

    [TearDown]
    public void CleanUp()
    {
        if (Directory.Exists(PersistenceOptions.Location)) Directory.Delete(PersistenceOptions.Location, true);
    }

    [Test]
    public async Task GetTotalVisitStatsAsync_Should_Classify_Stats()
    {
        // arrange
        var shortId = Guid.NewGuid();

        CreateVisit(new ShortVisitEntity(
            Guid.NewGuid(),
            shortId,
            "http://example.com",
            DateTimeOffset.UtcNow,
            "Referrer",
            "UserAgent 1"
        ), visitCount: 1);
        CreateVisit(new ShortVisitEntity(
            Guid.NewGuid(),
            shortId,
            "http://example.com",
            DateTimeOffset.UtcNow,
            "Referrer",
            "UserAgent 2"
        ), visitCount: 2);

        var calculatedStats = new ShortVisitsStats(shortId, 2);
        StatsCalculator
            .Setup(x => x.Calculate(shortId, It.IsAny<IEnumerable<ShortClassifiedVisit>>()))
            .Returns(calculatedStats);

        // act
        var stats = await Sut.GetTotalVisitStatsAsync(
            shortId
        );

        // assert
        Assert.That(
            stats,
            Is.Not.Null
                .And.Property(nameof(stats.TotalCount)).EqualTo(2)
        );

        StatsCalculator.VerifyAll();
        VisitClassifierMock
            .Verify(x => x.Classify(It.IsAny<ShortVisitEntity>()), Times.Exactly(2));
    }

    private void CreateVisit(
        ShortVisitEntity visit,
        int? visitCount = null
    )
    {
        visitCount ??= 1;

        var file = Path.Combine(PersistenceOptions.Location!, $"{visit.ShortId}-{visit.VisitedAt.ToUnixTimeSeconds()}-{visitCount}.json");
        using var fs = File.Open(file, FileMode.CreateNew);
        JsonSerializer.Serialize(fs, visit);
    }
}
