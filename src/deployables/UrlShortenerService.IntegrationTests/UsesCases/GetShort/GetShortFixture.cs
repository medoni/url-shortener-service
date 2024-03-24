using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortenerService.Domain.Short;
using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.UseCases.GetShort.Api;

namespace UrlShortenerService.IntegrationTests.UsesCases.GetShort;

[TestFixture]
[Category("Integration")]
public class GetShortFixture
{
    [Test]
    public async Task GetShort_Should_Redirect_To_Correct_Url()
    {
        // arrange
        var shortAggregate = new ShortAggregate(
            Guid.NewGuid(),
            "Foo",
            "Bar",
            "http://example.com"
        );
        await CreateAndPersistShortAsync(() => shortAggregate);

        var httpClient = TestEnvironment.CreateHttpClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/avif"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

        // act
        var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"api/shorts/{shortAggregate.Id}")
        );

        // assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Found));
        Assert.That(
            response.Headers.Select(x => (x.Key, x.Value.First())),
            Has.Some.EqualTo(("Location", "http://example.com/"))
        );
        Assert.That(
            await GetVisitCountFromShortAsync(shortAggregate.Id),
            Is.EqualTo(1)
        );
    }

    [Test]
    public async Task GetShort_Should_With_Accept_Json_Should_Return_Data()
    {
        // arrange
        var shortAggregate = new ShortAggregate(
            Guid.NewGuid(),
            "Foo",
            "Bar",
            "http://example.com"
        );
        await CreateAndPersistShortAsync(() => shortAggregate);

        var httpClient = TestEnvironment.CreateHttpClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // act
        var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"api/shorts/{shortAggregate.Id}")
        );

        // assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseDto = await response.Content.ReadFromJsonAsync<GetShortResponse>();

        Assert.That(
            responseDto,
            Is.Not.Null
                .And.Property(nameof(responseDto.Id)).EqualTo(shortAggregate.Id)
                .And.Property(nameof(responseDto.Title)).EqualTo("Foo")
                .And.Property(nameof(responseDto.Description)).EqualTo("Bar")
                .And.Property(nameof(responseDto.RedirectTo)).EqualTo("http://example.com")
        );
        Assert.That(
            await GetVisitCountFromShortAsync(shortAggregate.Id),
            Is.EqualTo(0)
        );
    }

    private async Task CreateAndPersistShortAsync(
        Func<ShortAggregate> createShortFn
    )
    {
        using var scopedSvcp = TestEnvironment.Services.CreateScope();
        var repo = scopedSvcp.ServiceProvider.GetRequiredService<IShortRepository>();

        var shortAggregate = createShortFn();
        await repo.AddAsync(shortAggregate);
    }

    private async Task<long> GetVisitCountFromShortAsync(Guid id)
    {
        using var scopedSvcp = TestEnvironment.Services.CreateScope();
        var repo = scopedSvcp.ServiceProvider.GetRequiredService<IShortVisitRepository>();

        var visitStats = await repo.GetTotalVisitStatsAsync(id);
        return visitStats.TotalCount;
    }
}
