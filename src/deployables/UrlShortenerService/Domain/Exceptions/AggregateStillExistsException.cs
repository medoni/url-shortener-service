namespace UrlShortenerService.Domain.Exceptions;

public class AggregateStillExistsException : Exception
{
    public AggregateStillExistsException(
        Type aggregateType,
        Guid aggregateId
    )
    : base($"The aggregate {aggregateType.Name} with id '{aggregateId}' still exists.")
    {
    }
}
