namespace UrlShortenerService.Persistence.FileBased.Configuration;

public record ShortVisitFileBasedPersistenceOptions
{
    public string? Location { get; set; }
}
