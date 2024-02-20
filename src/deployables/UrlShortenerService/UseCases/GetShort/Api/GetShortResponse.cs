using UrlShortenerService.Domain.Short;

namespace UrlShortenerService.UseCases.GetShort.Api;

public record GetShortResponse(
    Guid Id,
    string Title,
    string Description,
    string RedirectTo
)
{
    public GetShortResponse(
        ShortAggregate entity
    )
    : this(
        entity.Id,
        entity.Title,
        entity.Description,
        entity.RedirectTo
    )
    {
    }
}
