using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Services.ShortVisitClassifier;

namespace UrlShortenerService.UnitTests.Services.ShortVisitClassifier;

[TestFixture]
[Category("Unit")]
public class DefaultShortVisitClassifierFixture
{
    private DefaultShortVisitClassifier Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new DefaultShortVisitClassifier();
    }

    [Test]
    public void Classify_Should_Return_Correct_Result()
    {
        // arrange
        var visit = new ShortVisitEntity(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "http://example.com",
            new DateTime(2024, 03, 21, 06, 09, 23, DateTimeKind.Utc),
            "Referrer",
            "UserAgent"
        );

        // act
        var result = Sut.Classify(visit);

        // assert
        Assert.That(
            result,
            Is.EqualTo(new ShortClassifiedVisit())
        );
    }
}
