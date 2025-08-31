using NatoursApi.Domain.Entities;

namespace NatoursApi.Services.Abstractions;

public interface ITourStartDateService
{
    Task<IEnumerable<TourStartDate>> GetAllForTourAsync(Guid tourId, CancellationToken ct);

    Task<TourStartDate> GetByIdForTourAsync(Guid tourId, Guid startDateId, CancellationToken ct);

    Task<TourStartDate> CreateForTourAsync(Guid tourId, TourStartDate newTourStartDate, CancellationToken ct);

    Task<IEnumerable<TourStartDate>> CreateManyForTourAsync(Guid tourId, IEnumerable<TourStartDate> newStartDates,
        CancellationToken ct);

    Task<TourStartDate> UpdateForTourAsync(Guid tourId, TourStartDate newTourStartDate, CancellationToken ct);

    Task DeleteForTourAsync(Guid tourId, Guid startDateId, CancellationToken ct);
}