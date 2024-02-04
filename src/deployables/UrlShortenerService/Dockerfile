# build context /src

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY deployables/UrlShortenerService.sln ./deployables/
COPY deployables/UrlShortenerService/UrlShortenerService.csproj ./deployables/UrlShortenerService/
RUN dotnet restore deployables/UrlShortenerService/UrlShortenerService.csproj

COPY deployables/UrlShortenerService/** ./deployables//UrlShortenerService/
RUN dotnet publish deployables/UrlShortenerService/UrlShortenerService.csproj -c Release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/deployables/UrlShortenerService/bin/Release/net8.0/ .
ENTRYPOINT ["dotnet", "UrlShortenerService.dll"]