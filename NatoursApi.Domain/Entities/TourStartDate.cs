namespace NatoursApi.Domain.Entities;

public class TourStartDate
{
    public Guid Id { get; init; }

    public Guid TourId { get; init; }

    public DateTime Date { get; init; }

    // Navigation properties
    public Tour Tour { get; init; } = null!;
}