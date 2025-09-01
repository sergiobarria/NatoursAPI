using Microsoft.EntityFrameworkCore;
using NatoursApi.Data;
using NatoursApi.Domain.Entities;
using NatoursApi.Domain.Exceptions;
using NatoursApi.Services.Abstractions;

namespace NatoursApi.Services.Implementations;

public class TourStartDateService(ApplicationDbContext context) : ITourStartDateService
{
    public async Task<IEnumerable<TourStartDate>> GetAllForTourAsync(Guid tourId, CancellationToken ct)
    {
        var tourStartDates = await context.TourStartDates
            .AsNoTracking()
            .Where(tsd => tsd.TourId.Equals(tourId))
            .ToListAsync(ct);

        return tourStartDates;
    }

    public async Task<TourStartDate> GetByIdForTourAsync(Guid tourId, Guid startDateId, CancellationToken ct)
    {
        var tourStartDate = await context.TourStartDates
            .AsNoTracking()
            .FirstOrDefaultAsync(tsd => tsd.TourId.Equals(tourId) && tsd.Id.Equals(startDateId), ct);

        if (tourStartDate is null) throw new NotFoundException("TourStartDate", startDateId);

        return tourStartDate;
    }

    public async Task<TourStartDate> CreateForTourAsync(Guid tourId, TourStartDate newStartDate,
        CancellationToken ct)
    {
        var tourExists = await context.Tours.AnyAsync(t => t.Id.Equals(tourId), ct);
        if (!tourExists) throw new NotFoundException("Tour", tourId);

        newStartDate.Id = Guid.CreateVersion7();
        newStartDate.TourId = tourId;

        context.TourStartDates.Add(newStartDate);
        await context.SaveChangesAsync(ct);

        return newStartDate;
    }

    public async Task<IEnumerable<TourStartDate>> CreateManyForTourAsync(
        Guid tourId,
        IEnumerable<TourStartDate> newStartDates, CancellationToken ct
    )
    {
        var tourExists = await context.Tours.AnyAsync(t => t.Id.Equals(tourId), ct);
        if (!tourExists) throw new NotFoundException("Tour", tourId);

        var startDatesList = newStartDates.ToList();

        foreach (var startDate in startDatesList)
        {
            startDate.Id = Guid.CreateVersion7();
            startDate.TourId = tourId;
        }

        context.TourStartDates.AddRange(startDatesList);
        await context.SaveChangesAsync(ct);
        return startDatesList;
    }

    public async Task<TourStartDate> UpdateForTourAsync(Guid tourId, TourStartDate newTourStartDate,
        CancellationToken ct)
    {
        var existingStartDate =
            await context.TourStartDates.FirstOrDefaultAsync(
                tsd => tsd.TourId.Equals(tourId) && tsd.Id.Equals(newTourStartDate.Id), ct);
        if (existingStartDate is null) throw new NotFoundException("TourStartDate", newTourStartDate.Id);

        existingStartDate.Date = newTourStartDate.Date;

        await context.SaveChangesAsync(ct);
        return existingStartDate;
    }

    public async Task DeleteForTourAsync(Guid tourId, Guid startDateId, CancellationToken ct)
    {
        var startDateToDelete =
            await context.TourStartDates.FirstOrDefaultAsync(
                tsd => tsd.TourId.Equals(tourId) && tsd.Id.Equals(startDateId), ct);
        if (startDateToDelete is null) throw new NotFoundException("TourStartDate", startDateId);

        context.TourStartDates.Remove(startDateToDelete);
        await context.SaveChangesAsync(ct);
    }
}