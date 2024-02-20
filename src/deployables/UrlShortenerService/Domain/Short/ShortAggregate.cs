namespace UrlShortenerService.Domain.Short;

public class ShortAggregate
{
    private readonly ShortAggregateState State;

    public Guid Id
    {
        get => State.Id;
        private set => State.Id = value;
    }
    public string Title
    {
        get => State.Title;
        private set => State.Title = value;
    }
    public string Description
    {
        get => State.Description;
        private set => State.Description = value;
    }
    public string RedirectTo
    {
        get => State.RedirectTo;
        private set => State.RedirectTo = value;
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

        State = new ShortAggregateState(
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
        State = state ?? throw new ArgumentNullException(nameof(state));
    }
}