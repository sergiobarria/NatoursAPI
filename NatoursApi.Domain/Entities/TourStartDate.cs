namespace NatoursApi.Domain.Entities;

public class TourStartDate
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public DateTime Date { get; set; }

    // Navigation properties
    public Tour Tour { get; init; } = null!;
}