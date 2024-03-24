namespace UrlShortenerService.UseCases.CreateShort.Api;

public record CreateShortRequestDto(
    string RedirectTo,
    string Title,
    string Description
);