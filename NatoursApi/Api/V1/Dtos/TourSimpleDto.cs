namespace NatoursApi.Api.V1.Dtos;

public record TourSimpleDto(
    Guid Id,
    string Name,
    decimal Price,
    decimal RatingsAverage,
    decimal Summary
);