namespace NatoursApi.Features.Tours.v1;

public interface ITourService
{
    Task<List<TourDto>> GetAllAsync(CancellationToken ct);
}