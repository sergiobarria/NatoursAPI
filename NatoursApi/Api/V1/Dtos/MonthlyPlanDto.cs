namespace NatoursApi.Api.V1.Dtos;

public record MonthlyPlanDto(
    string Month,
    int NumTourStarts,
    List<string> Tours
);