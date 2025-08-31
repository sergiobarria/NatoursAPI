using NatoursApi.Domain.Entities;

namespace NatoursApi.Services.Abstractions;

public interface ITourService
{
    Task<IEnumerable<Tour>> GetAllAsync(CancellationToken ct);

    Task<Tour> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Tour> CreateAsync(Tour newTour, CancellationToken ct);

    Task<Tour> UpdateAsync(Guid id, Tour updatedTour, CancellationToken ct);

    Task DeleteAsync(Guid id, CancellationToken ct);
}