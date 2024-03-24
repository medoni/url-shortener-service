using FastEndpoints;
using UrlShortenerService.Domain.ShortStat;
using UrlShortenerService.Services.ShortVisitClassifier;

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

        Description(d => d
            .Produces<GetShortStatsResponse>(StatusCodes.Status201Created, "application/json")
        );

        Summary(s =>
        {
            s.Summary = "Returns stat information about a single short visit";
            s.Description = "Returns stat information about a single short visit with classified statistical informations.";
            s.Responses[200] = "Stat information for the given short.";
            s.ExampleRequest = new GetShortStatsRequest(Guid.Parse("7a614d94-b7fe-47b0-93ea-6ed908fbc5d9"));
            s.ResponseExamples[200] = new GetShortStatsResponse(
                Guid.Parse("7a614d94-b7fe-47b0-93ea-6ed908fbc5d9"),
                42,
                23,
                new()
                {
                    new Dtos.ClassifiedVisitItemDto(ClassifiedTypes.BrowserType, "Firefox", 10),
                    new Dtos.ClassifiedVisitItemDto(ClassifiedTypes.RegionA, "NRW, Germany", 3)
                }
            );
        });
    }

    // todo: unit tests
    public override async Task HandleAsync(GetShortStatsRequest req, CancellationToken ct)
    {
        var stat = await _shortStatRepository.GetTotalVisitStatsAsync(req.Id);
        var response = new GetShortStatsResponse(
            stat.ShortId,
            stat.TotalCount,
            0,
            stat.Items
                .Select(x => new Dtos.ClassifiedVisitItemDto(x.Key.Type, x.Key.Value, x.Value))
                .ToList()
        );

        await SendAsync(response, cancellation: ct);
    }
}
