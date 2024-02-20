using FastEndpoints;
using FastEndpoints.Swagger;
using UrlShortenerService.UseCases;

namespace UrlShortenerService;

public static class Program
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
            .AddFastEndpoints()
            .AddUseCases()
        ;

        var app = builder.Build();

        app
            .UseFastEndpoints()
            .UseSwaggerGen()
        ;

        app.Run();
    }
}
