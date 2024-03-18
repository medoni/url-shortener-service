using FastEndpoints;
using FastEndpoints.Swagger;
using UrlShortenerService.Persistence;
using UrlShortenerService.Persistence.FileBased;
using UrlShortenerService.Services.ShortVisitClassifier;
using UrlShortenerService.Services.ShortVisitStatsCalculator;
using UrlShortenerService.UseCases;

namespace UrlShortenerService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.SwaggerDocument(o =>
            o.DocumentSettings = s =>
            {
                s.Title = "Url shortener service";
                s.Version = "v1";
            }
        );

        builder.Services
            .AddShortsPersistence(
                opt => opt.AddFileBased(fileBasedOpt => builder.Configuration.Bind("UrlShortenerService:Persistence:Shorts:FileBasedPersistence", fileBasedOpt))
            )
            .AddShortStatsPersistence(
                opt => opt.AddFileBased(fileBasedOpt => builder.Configuration.Bind("UrlShortenerService:Persistence:ShortVisits:FileBasedPersistence", fileBasedOpt))
            );

        builder.Services
            .AddFastEndpoints()
            .AddUseCases()
            .AddShortVisitClassifier()
            .AddShortVisitStatsCalculator()
        ;

        var app = builder.Build();

        app
            .UseFastEndpoints()
            .UseSwaggerGen()
        ;

        app.Run();
    }
}
