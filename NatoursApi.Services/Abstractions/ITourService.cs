using NatoursApi.Domain.Entities;
using Shared.RequestFeatures;

namespace NatoursApi.Services.Abstractions;

public interface ITourService
{
    Task<PagedList<Tour>> GetAllAsync(TourQueryParameters queryParameters, CancellationToken ct);

    Task<Tour> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Tour> CreateAsync(Tour newTour, CancellationToken ct);

    Task<Tour> UpdateAsync(Guid id, Tour updatedTour, CancellationToken ct);

    Task DeleteAsync(Guid id, CancellationToken ct);

    Task<IEnumerable<Tour>> GetTopToursAsync(CancellationToken ct);

    Task<object> GetTourStatsAsync(CancellationToken ct);

    Task<object> GetMonthlyPlanAsync(int year, CancellationToken ct);
}