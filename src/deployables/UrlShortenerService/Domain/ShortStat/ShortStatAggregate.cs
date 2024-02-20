namespace UrlShortenerService.Domain.ShortStat;

public class ShortStatAggregate
{
    private readonly ShortStatAggregateState State;

    public Guid Id
    {
        get => State.Id;
        private set => State.Id = value;
    }

    public long VisitTotalCount
    {
        get => State.VisitTotalCount;
        private set => State.VisitTotalCount = value;
    }

    public long Visit24Hours
    {
        get => State.Visit24Hours;
        set => State.Visit24Hours = value;
    }

    public ShortStatAggregate(
        Guid id
    )
    {
        State = new ShortStatAggregateState(id);
    }

    public ShortStatAggregate(
        ShortStatAggregateState state
    )
    {
        State = state ?? throw new ArgumentNullException(nameof(state));
    }
}
