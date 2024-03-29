namespace UrlShortenerService.Domain.Exceptions;

public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException(
        Type aggregateType,
        Guid aggregateId
    )
    : base($"The aggregate {aggregateType.Name} with id '{aggregateId}' was not found.")
    {
    }
}
