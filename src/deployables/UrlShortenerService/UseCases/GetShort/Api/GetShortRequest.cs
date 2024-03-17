using FastEndpoints;

namespace UrlShortenerService.UseCases.GetShort.Api;

public record GetShortRequest(
    Guid Id
)
{
    [FromHeader("Accept", IsRequired = false)]
    public IList<string>? AcceptHeaders { get; init; }
}
