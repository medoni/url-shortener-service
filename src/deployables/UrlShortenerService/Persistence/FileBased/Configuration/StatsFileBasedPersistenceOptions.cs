namespace UrlShortenerService.Persistence.FileBased.Configuration;

public record StatsFileBasedPersistenceOptions
{
    public string? Location { get; set; }
}
