using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UrlShortenerService.Persistence.FileBased.Configuration;

namespace UrlShortenerService.IntegrationTests;

[SetUpFixture]
public class TestEnvironment
{
    private static TestEnvironmentApplicationFactory<Program> ApplicationFactory { get; set; }
    public static IServiceProvider Services => ApplicationFactory.Services;

    [OneTimeSetUp]
    public void Setup()
    {
        ApplicationFactory = new TestEnvironmentApplicationFactory<Program>();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        var filesAndFolderToCleanUp = new[] {
            ApplicationFactory.Services.GetRequiredService<IOptions<StatsFileBasedPersistenceOptions>>().Value.Location,
            ApplicationFactory.Services.GetRequiredService<IOptions<ShortVisitFileBasedPersistenceOptions>>().Value.Location
        };

        await ApplicationFactory.DisposeAsync();

        DeleteFilesAndFoldersIfExists(filesAndFolderToCleanUp);
    }

    public static HttpClient CreateHttpClient(
    )
    {
        var client = ApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        return client;
    }

    private static void DeleteFilesAndFoldersIfExists(
        IReadOnlyList<string?> filesAndFolders
    )
    {
        foreach (var item in filesAndFolders)
        {
            if (File.Exists(item))
            {
                File.Delete(item);
            }
            else if (Directory.Exists(item))
            {
                Directory.Delete(item, true);
            }
        }
    }
}
