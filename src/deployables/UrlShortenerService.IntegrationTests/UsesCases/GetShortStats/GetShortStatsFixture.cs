using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortenerService.Domain.Short;
using UrlShortenerService.UseCases.GetShortStats.Api;

namespace UrlShortenerService.IntegrationTests.UsesCases.GetShortStats;

[TestFixture]
[Category("Unit")]
public class GetShortStatsFixtureTest
{
    [Test]
    public async Task Visited_Shorts_Should_Return_Short_Stats()
    {
        // arrange
        var faker = new Faker();

        var shorts = await Enumerable.Range(1, 3)
            .ToAsyncEnumerable()
            .SelectAwait(async num => await CreateAndPersistShortAsync($"title {num}.", $"http://example.com/{num}"))
            .ToArrayAsync();

        var visits = shorts
            .Select(async shortId => await CreateVisitAsync(
                shortId,
                faker.PickRandom(Referrers),
                faker.PickRandom(UserAgents)
            ));
        await Task.WhenAll(visits);

        // act
        var visitStats = await shorts
            .ToAsyncEnumerable()
            .SelectAwait(async shortId => await GetVisitStatsAsync(shortId))
            .ToArrayAsync();

        // assert
        Assert.That(
            visitStats,
            Has.Exactly(shorts.Length).Items
                .And.All.Property(nameof(GetShortStatsResponse.VisitTotalCount)).EqualTo(1)
        );
    }

    private async Task<Guid> CreateAndPersistShortAsync(
        string title,
        string redirect
    )
    {
        using var scopedSvcp = TestEnvironment.Services.CreateScope();
        var repo = scopedSvcp.ServiceProvider.GetRequiredService<IShortRepository>();

        var shortAggregate = new ShortAggregate(
            Guid.NewGuid(),
            title,
            "Some Descr",
            redirect
        );
        await repo.AddAsync(shortAggregate);

        return shortAggregate.Id;
    }

    private async Task CreateVisitAsync(
        Guid shortId,
        string? referrer,
        string? userAgent
    )
    {
        var httpClient = TestEnvironment.CreateHttpClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/avif"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

        if (referrer != null)
        {
            httpClient.DefaultRequestHeaders.Add("Referrer", referrer);
        }

        if (userAgent != null)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        // act
        var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"api/shorts/{shortId}")
        );

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Found));
        response.Headers.TryGetValues("Location", out var locationHeaders);
        Assert.That(locationHeaders, Has.One.StartsWith($"http://example.com/"));
    }

    private async Task<GetShortStatsResponse> GetVisitStatsAsync(
        Guid shortId
    )
    {
        var response = await TestEnvironment.CreateHttpClient().SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/api/shorts/{shortId}/stats")
        );

        var jsonRes = await response.Content.ReadFromJsonAsync<GetShortStatsResponse>();
        return jsonRes!;
    }

    private static readonly string?[] UserAgents = new[]
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 Edg/119.0.0.0",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/119.0",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.5 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (iPad; CPU OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.5 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (Linux; Android 10; K) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Mobile Safari/537.36",
        "Mozilla/5.0 (Linux; Android 13; SM-S901B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Mobile Safari/537.36",
        "curl/7.54.1",
        null
    };

    private static readonly string?[] Referrers = new[]
    {
        "https://www.reddit.com/",
        "https://news.ycombinator.com/",
        "https://stackoverflow.com/?tag=csharp",
        "https://github.com/",
        "https://medium.com/?category=technology",
        null
    };
}
