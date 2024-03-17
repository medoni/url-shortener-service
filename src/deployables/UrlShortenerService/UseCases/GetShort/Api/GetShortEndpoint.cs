using FastEndpoints;
using System.Net.Http.Headers;
using UrlShortenerService.Domain.Short;

namespace UrlShortenerService.UseCases.GetShort.Api;

public class GetShortEndpoint : Endpoint<GetShortRequest, GetShortResponse>
{
    private readonly IShortRepository _shortRepository;

    public GetShortEndpoint(IShortRepository shortRepository)
    {
        _shortRepository = shortRepository ?? throw new ArgumentNullException(nameof(shortRepository));
    }

    public override void Configure()
    {
        Get("/api/shorts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetShortRequest req, CancellationToken ct)
    {
        var entity = await _shortRepository.GetByIdAsync(req.Id);

        await SendAsyncByAcceptAsync(
            req,
            new()
            {
                ["text/html"] = async () =>
                {
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

    private Task SendAsyncByAcceptAsync(
        GetShortRequest req,
        Dictionary<string, Func<Task>> sendByAccept
    )
    {
        var defaultAction = sendByAccept.First().Value;
        if (req.AcceptHeaders is null) return defaultAction.Invoke();

        foreach (var acceptHeader in req.AcceptHeaders)
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
}