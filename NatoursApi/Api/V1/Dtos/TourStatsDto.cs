namespace NatoursApi.Api.V1.Dtos;

public record TourStatsDto(
    string Difficulty,
    int NumTours,
    int NumRatings,
    decimal AvgRating,
    decimal AvgPrice,
    decimal MinPrice,
    decimal MaxPrice
);