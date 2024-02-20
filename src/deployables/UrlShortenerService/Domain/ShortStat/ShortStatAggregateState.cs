namespace UrlShortenerService.Domain.ShortStat;

public record ShortStatAggregateState
{
    public Guid Id { get; set; }
    public long VisitTotalCount { get; set; }
    public long Visit24Hours { get; set; }

    public ShortStatAggregateState(Guid id)
    {
        Id = id;
    }

    [Obsolete("Deserialization only")]
    public ShortStatAggregateState()
    {
    }
}
