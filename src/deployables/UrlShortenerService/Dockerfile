# build context /src

# dotnet sdk build layer
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY deployables/Directory.Build.props ./deployables/
COPY deployables/UrlShortenerService.sln ./deployables/
COPY deployables/UrlShortenerService/UrlShortenerService.csproj ./deployables/UrlShortenerService/
RUN dotnet restore deployables/UrlShortenerService/UrlShortenerService.csproj

COPY deployables/UrlShortenerService/** ./deployables//UrlShortenerService/
RUN dotnet publish deployables/UrlShortenerService/UrlShortenerService.csproj -c Release --no-restore


# dotnet runtime layer
FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt-get update \
    && apt-get install -y curl

WORKDIR /app
COPY --from=build /app/deployables/UrlShortenerService/bin/Release/net6.0/ .

HEALTHCHECK CMD curl --fail http://localhost/Swagger/index.html || exit 1   

ENTRYPOINT ["dotnet", "UrlShortenerService.dll"]