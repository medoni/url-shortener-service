using FastEndpoints;
using System.Net.Http.Headers;
using UrlShortenerService.Domain.Short;
using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.UseCases.GetShort.Api;

public class GetShortEndpoint : Endpoint<GetShortRequest, GetShortResponse>
{
    private readonly IShortRepository _shortRepository;
    private readonly IShortVisitRepository _shortStatRepository;

    public GetShortEndpoint(
        IShortRepository shortRepository,
        IShortVisitRepository shortStatRepository
    )
    {
        _shortRepository = shortRepository ?? throw new ArgumentNullException(nameof(shortRepository));
        _shortStatRepository = shortStatRepository ?? throw new ArgumentNullException(nameof(shortStatRepository));
    }

    public override void Configure()
    {
        Get("/api/shorts/{id}");
        AllowAnonymous();

        Description(d => d
            .Produces<GetShortResponse>(StatusCodes.Status200OK, "application/json")
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status302Found, contentType: "text/html")
        );
        Summary(s =>
        {
            s.Summary = "Retrieves information about a stored short of an Url.";
            s.Description = "Returns json information about a stored short of an Url. Or returns a http found (302) with location header if `Accept: text/html` was set in the request.";
            s.Responses[200] = "Short with the given id was found. (`Accept: application/json`)";
            s.Responses[404] = "Short with the given id was not found.";
            s.Responses[302] = "Short with the given id was found. (`Accept: text/html`). `Location` with the redirect to url is present.";
            s.ResponseExamples[200] = new GetShortResponse(
                Guid.Parse("7a614d94-b7fe-47b0-93ea-6ed908fbc5d9"),
                "Title of example.com",
                "Description of example.com",
                "http://example.com"
            );
        });
    }

    // todo: unit tests
    public override async Task HandleAsync(GetShortRequest shortRequest, CancellationToken ct)
    {
        var entity = await _shortRepository.GetByIdAsync(shortRequest.Id);

        await SendByAcceptHeaderAsync(
            new()
            {
                ["text/html"] = async () =>
                {
                    await RecordVisitAsync(entity);

                    await SendRedirectAsync(
                        entity.RedirectTo,
                        allowRemoteRedirects: true
                    );
                },
                ["application/json"] = async () =>
                {
                    var responseDto = entity.ToResponseDto();
                    await SendAsync(
                        responseDto,
                        StatusCodes.Status200OK
                    );
                }
            }
        );
    }

    private Task SendByAcceptHeaderAsync(
        Dictionary<string, Func<Task>> sendByAccept
    )
    {
        var httpRequest = HttpContext.Request;
        var defaultAction = sendByAccept.First().Value;
        if (!httpRequest.Headers.Accept.Any()) return defaultAction.Invoke();

        foreach (var acceptHeader in httpRequest.Headers.Accept)
        {
            if (!MediaTypeWithQualityHeaderValue.TryParse(acceptHeader, out var parsedMediaValue)) continue;
            if (parsedMediaValue.MediaType is null) continue;

            if (sendByAccept.TryGetValue(parsedMediaValue.MediaType, out var foundAction))
            {
                return foundAction.Invoke();
            }
        }

        return defaultAction.Invoke();
    }

    private async Task RecordVisitAsync(
        ShortAggregate shortAggregate
    )
    {
        var httpRequest = HttpContext.Request;

        var visit = new ShortVisitEntity(
            Guid.NewGuid(),
            shortAggregate.Id,
            shortAggregate.RedirectTo,
            DateTimeOffset.UtcNow,
            httpRequest.Headers["Referrer"].FirstOrDefault(),
            httpRequest.Headers["User-Agent"].FirstOrDefault()
        );
        await _shortStatRepository.AddAsync(visit);
    }
}