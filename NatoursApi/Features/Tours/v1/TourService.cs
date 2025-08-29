using Mapster;
using Microsoft.EntityFrameworkCore;
using NatoursApi.Data;

namespace NatoursApi.Features.Tours.v1;

public class TourService(ApplicationDbContext db) : ITourService
{
    public async Task<List<TourDto>> GetAllAsync(CancellationToken ct)
    {
        var tours = await db.Tours
            .AsNoTracking()
            .Include(t => t.StartDates)
            .OrderBy(t => t.Name)
            .ToListAsync(ct);

        var mappedTours = tours.Adapt<List<TourDto>>();

        return mappedTours;
    }
}