namespace UrlShortenerService.Domain.Short;

public class ShortAggregate : IAggregateRootWithPersistenceState<ShortAggregateState>
{
    private readonly ShortAggregateState Persistence;
    ShortAggregateState IAggregateRootWithPersistenceState<ShortAggregateState>.Persistence => Persistence;

    public Guid Id
    {
        get => Persistence.Id;
        private set => Persistence.Id = value;
    }
    public string Title
    {
        get => Persistence.Title;
        private set => Persistence.Title = value;
    }
    public string Description
    {
        get => Persistence.Description;
        private set => Persistence.Description = value;
    }
    public string RedirectTo
    {
        get => Persistence.RedirectTo;
        private set => Persistence.RedirectTo = value;
    }

    public ShortAggregate(
        Guid id,
        string title,
        string description,
        string redirectTo
    )
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
        if (string.IsNullOrEmpty(description)) throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));
        if (string.IsNullOrEmpty(redirectTo)) throw new ArgumentException($"'{nameof(redirectTo)}' cannot be null or empty.", nameof(redirectTo));

        Persistence = new ShortAggregateState(
            id,
            title,
            description,
            redirectTo
        );
    }

    public ShortAggregate(
        ShortAggregateState state
    )
    {
        Persistence = state ?? throw new ArgumentNullException(nameof(state));
    }
}