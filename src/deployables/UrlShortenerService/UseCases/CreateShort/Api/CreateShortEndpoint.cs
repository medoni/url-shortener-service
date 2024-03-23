using FastEndpoints;
using UrlShortenerService.Domain.Short;

namespace UrlShortenerService.UseCases.CreateShort.Api;

public class CreateShortEndpoint : Endpoint<CreateShortRequestDto, CreateShortResponseDto>
{
    private readonly IShortRepository _shortRepository;

    public CreateShortEndpoint(IShortRepository shortRepository)
    {
        _shortRepository = shortRepository ?? throw new ArgumentNullException(nameof(shortRepository));
    }

    public override void Configure()
    {
        Post("/api/shorts/");
        AllowAnonymous();

        Description(d => d
            .Produces<CreateShortResponseDto>(StatusCodes.Status201Created, "application/json")
        );

        Summary(s =>
        {
            s.Summary = "Creates a new short for a given url and their meta data";
            s.Description = "Creates a new short for a given url and their meta data";
            s.Responses[201] = "Returns 201 when the short was successfully created.";
            s.ExampleRequest = new CreateShortRequestDto("http://example.com", "Example Website", "Short url for Example.com");
            s.ResponseExamples[201] = new CreateShortResponseDto(
                Guid.Parse("7a614d94-b7fe-47b0-93ea-6ed908fbc5d9")
            );
        });
    }

    // todo: unit tests
    public override async Task HandleAsync(CreateShortRequestDto req, CancellationToken ct)
    {
        var entity = new ShortAggregate(
            Guid.NewGuid(),
            req.Title,
            req.Description,
            req.RedirectTo
        );

        await _shortRepository.AddAsync(entity);

        await SendAsync(
            new CreateShortResponseDto(entity.Id),
            StatusCodes.Status201Created
        );
    }
}
