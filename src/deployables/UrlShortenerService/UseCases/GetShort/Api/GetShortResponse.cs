namespace UrlShortenerService.UseCases.GetShort.Api;

public record GetShortResponse(
    Guid Id,
    string Title,
    string Description,
    string RedirectTo
)
{
}
