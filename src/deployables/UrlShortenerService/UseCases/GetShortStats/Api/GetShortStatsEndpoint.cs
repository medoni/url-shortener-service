using FastEndpoints;
using UrlShortenerService.Domain.ShortStat;

namespace UrlShortenerService.UseCases.GetShortStats.Api;

public class GetShortStatsEndpoint : Endpoint<GetShortStatsRequest, GetShortStatsResponse>
{
    private readonly IShortVisitRepository _shortStatRepository;

    public GetShortStatsEndpoint(IShortVisitRepository shortStatRepository)
    {
        _shortStatRepository = shortStatRepository ?? throw new ArgumentNullException(nameof(shortStatRepository));
    }

    public override void Configure()
    {
        Get("/api/shorts/{id}/stats");
        AllowAnonymous();
    }

    public override Task HandleAsync(GetShortStatsRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
