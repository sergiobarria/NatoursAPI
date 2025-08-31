namespace NatoursApi.Api.V1.Dtos;

public record UpdateTourDto(
    string Name,
    int Duration,
    int MaxGroupSize,
    string Difficulty,
    decimal Price,
    decimal? PriceDiscount,
    string Summary,
    string? Description
);