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
    }

    public override async Task HandleAsync(CreateShortRequestDto req, CancellationToken ct)
    {
        var entity = new ShortAggregate(
            Guid.NewGuid(),
            req.Title,
            req.Description,
            req.Title
        );

        await _shortRepository.AddAsync(entity);

        await SendAsync(
            new CreateShortResponseDto(entity.Id),
            StatusCodes.Status201Created
        );
    }
}
