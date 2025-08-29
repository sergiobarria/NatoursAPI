namespace NatoursApi.Features.Tours.v1;

public record TourDto(
    Guid Id,
    string Name,
    string Slug,
    int Duration,
    int MaxGroupSize,
    string Difficulty,
    decimal RatingsAverage,
    int RatingsQuantity,
    decimal Price,
    string Summary,
    string? Description,
    List<DateTime> StartDates
);