using FastEndpoints;
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

        await SendAsync(
            new GetShortResponse(entity),
            StatusCodes.Status200OK
        );
    }
}