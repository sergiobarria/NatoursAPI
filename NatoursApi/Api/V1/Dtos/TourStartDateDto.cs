namespace NatoursApi.Api.V1.Dtos;

public record TourStartDateDto(
    Guid Id,
    DateTime Date,
    Guid TourId
);