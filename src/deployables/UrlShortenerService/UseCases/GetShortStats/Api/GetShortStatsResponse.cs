using UrlShortenerService.UseCases.GetShortStats.Api.Dtos;

namespace UrlShortenerService.UseCases.GetShortStats.Api;

public record GetShortStatsResponse(
    Guid Id,
    long VisitTotalCount,
    long Visit24Hours,
    List<ClassifiedVisitItemDto> ClassifiedItems
);
