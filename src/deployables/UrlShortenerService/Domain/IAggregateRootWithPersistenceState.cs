namespace UrlShortenerService.Domain;

public interface IAggregateRootWithPersistenceState<TAggregateState> : IAggregateRoot
where TAggregateState : class, new()
{
    TAggregateState Persistence { get; }
}
