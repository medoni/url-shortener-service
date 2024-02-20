using FastEndpoints;
using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.UseCases.GetShortStats.Api;

public class GetShortStatsEndpoint : Endpoint<GetShortStatsRequest, GetShortStatsResponse>
{
    private readonly IShortStatRepository _shortStatRepository;

    public GetShortStatsEndpoint(IShortStatRepository shortStatRepository)
    {
        _shortStatRepository = shortStatRepository ?? throw new ArgumentNullException(nameof(shortStatRepository));
    }

    public override void Configure()
    {
        Get("/api/shorts/{id}/stats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetShortStatsRequest req, CancellationToken ct)
    {
        var entity = await _shortStatRepository.GetByIdAsync(req.Id);

        await SendAsync(
            new GetShortStatsResponse(entity),
            StatusCodes.Status200OK
        );
    }
}
