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
            new ShortClassifiedVisit(
                new[] {
                    new ClassifiedItem(ClassifiedTypes.BrowserType, "Firefox"),
                    new ClassifiedItem(ClassifiedTypes.RegionA, "NRW")
                }
            ),
            new ShortClassifiedVisit(
                new[] {
                    new ClassifiedItem(ClassifiedTypes.BrowserType, "Firefox"),
                    new ClassifiedItem(ClassifiedTypes.RegionA, "Berlin")
                }
            )
        };

        // act
        var result = Sut.Calculate(
            shortId,
            classifiedVisits
        );

        // assert
        Assert.That(
            result,
            Is.Not.Null
                .And.Property(nameof(result.ShortId)).EqualTo(shortId)
                .And.Property(nameof(result.TotalCount)).EqualTo(2)
                .And.Property(nameof(result.Items)).EquivalentTo(
                    new Dictionary<(ClassifiedTypes Type, string Value), long>
                    {
                        [(Type: ClassifiedTypes.BrowserType, Value: "Firefox")] = 2,
                        [(Type: ClassifiedTypes.RegionA, Value: "NRW")] = 1,
                        [(Type: ClassifiedTypes.RegionA, Value: "Berlin")] = 1
                    }
                )
        );
    }
}
