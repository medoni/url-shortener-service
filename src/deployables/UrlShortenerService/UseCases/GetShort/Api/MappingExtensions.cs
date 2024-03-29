using UrlShortenerService.Domain.Short;

namespace UrlShortenerService.UseCases.GetShort.Api;

internal static class MappingExtensions
{
    public static GetShortResponse ToResponseDto(
        this ShortAggregate aggregate
    )
    => new GetShortResponse(
        aggregate.Id,
        aggregate.Title,
        aggregate.Description,
        aggregate.RedirectTo
    );
}
