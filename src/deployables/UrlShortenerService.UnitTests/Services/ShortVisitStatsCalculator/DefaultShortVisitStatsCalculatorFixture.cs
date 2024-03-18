using UrlShortenerService.Domain.ShortVisit;
using UrlShortenerService.Services.ShortVisitClassifier;
using UrlShortenerService.Services.ShortVisitStatsCalculator;

namespace UrlShortenerService.UnitTests.Services.ShortVisitStatsCalculator;

[TestFixture]
[Category("Unit")]
public class DefaultShortVisitStatsCalculatorFixture
{
    private DefaultShortVisitStatsCalculator Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new DefaultShortVisitStatsCalculator();
    }

    [Test]
    public void Calculate_Should_Return_Correct_Results()
    {
        // arrange
        var shortId = Guid.NewGuid();
        var classifiedVisits = new[]
        {
            new ShortClassifiedVisit(),
            new ShortClassifiedVisit()
        };

        // act
        var results = Sut.Calculate(
            shortId,
            classifiedVisits
        );

        // assert
        Assert.That(
            results,
            Is.EqualTo(new ShortVisitsStats(shortId, 2))
        );
    }
}
