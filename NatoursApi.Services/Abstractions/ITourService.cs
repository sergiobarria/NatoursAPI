using NatoursApi.Domain.Entities;

namespace NatoursApi.Services.Abstractions;

public interface ITourService
{
    Task<IEnumerable<Tour>> GetAllAsync(CancellationToken ct);

    Task<Tour> GetByIdAsync(Guid id, CancellationToken ct);
}