using Microsoft.EntityFrameworkCore;
using NatoursApi.Data;
using NatoursApi.Domain.Entities;
using NatoursApi.Domain.Exceptions;
using NatoursApi.Services.Abstractions;

namespace NatoursApi.Services.Implementations;

public class TourService(ApplicationDbContext context) : ITourService
{
    public async Task<IEnumerable<Tour>> GetAllAsync(CancellationToken ct)
    {
        var tours = await context.Tours
            .AsNoTracking()
            .Include(t => t.StartDates)
            .OrderBy(t => t.Name)
            .ToListAsync(ct);

        return tours;
    }

    public async Task<Tour> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var tour = await context.Tours
            .AsNoTracking()
            .Include(t => t.StartDates)
            .FirstOrDefaultAsync(t => t.Id.Equals(id), ct);

        if (tour is null) throw new NotFoundException("Tour", id);

        return tour;
    }
}