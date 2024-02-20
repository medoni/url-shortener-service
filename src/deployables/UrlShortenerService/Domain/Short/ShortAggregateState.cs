namespace UrlShortenerService.Domain.Short;

public record ShortAggregateState
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RedirectTo { get; set; }

    public ShortAggregateState(
        Guid id,
        string title,
        string description,
        string redirectTo
    )
    {
        Id = id;
        Title = title;
        Description = description;
        RedirectTo = redirectTo;
    }

    [Obsolete("Deserialization only")]
    public ShortAggregateState()
    {
        Title = null!;
        Description = null!;
        RedirectTo = null!;
    }
}
