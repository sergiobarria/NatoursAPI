namespace NatoursApi.Controllers.V1.Dtos;

public record CreateTourDto(
    string Name,
    int Duration,
    int MaxGroupSize,
    string Difficulty,
    decimal Price,
    decimal? PriceDiscount,
    string Summary,
    string? Description
);