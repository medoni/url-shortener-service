namespace UrlShortenerService.Domain;

public interface IAggregateRootWithPersistenceState<TAggregateState> : IAggregateRoot
where TAggregateState : class
{
    TAggregateState Persistence { get; }
}
