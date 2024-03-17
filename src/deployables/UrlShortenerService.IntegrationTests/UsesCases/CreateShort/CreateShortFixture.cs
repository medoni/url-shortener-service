using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;
using UrlShortenerService.Domain.Short;
using UrlShortenerService.UseCases.CreateShort.Api;

namespace UrlShortenerService.IntegrationTests.UsesCases.CreateShort;

[TestFixture]
[Category("Unit")]
public class CreateShortFixtureTests
{
    [Test]
    public async Task CreateShort_Should_Persist_Data()
    {
        // arrange
        var httpClient = TestEnvironment.CreateHttpClient();

        // act
        var response = await httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "api/shorts/")
            {
                Content = new StringContent(
                    """
                    {
                        "redirectTo": "http://example.com",
                        "description": "Lorem Ipsum",
                        "title": "Foo Bar"
                    }
                    """,
                    Encoding.UTF8, "application/json"
                )
            }
        );

        // assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var shortAggregate = await GetShortAggregateByResponse<CreateShortResponseDto>(
            response,
            json => json.Id
        );

        Assert.That(
            shortAggregate,
            Is.Not.Null
                .And.Property(nameof(ShortAggregate.Id)).Not.EqualTo(Guid.Empty)
                .And.Property(nameof(ShortAggregate.Title)).EqualTo("Foo Bar")
                .And.Property(nameof(ShortAggregate.Description)).EqualTo("Lorem Ipsum")
        );
    }

    private async Task<ShortAggregate> GetShortAggregateByResponse<TJsonResponse>(
        HttpResponseMessage response,
        Func<TJsonResponse, Guid> idExtractor
    )
    {
        using var scopedSvcp = TestEnvironment.Services.CreateScope();

        var rawResponse = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<TJsonResponse>(rawResponse, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var id = idExtractor(json!);

        var repo = scopedSvcp.ServiceProvider.GetRequiredService<IShortRepository>();
        var shortAggregate = await repo.GetByIdAsync(id);

        return shortAggregate;
    }
}
