using System.Text.RegularExpressions;
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

        if (tour is null) throw new NotFoundException($"Tour with id {id} not found.");

        return tour;
    }

    public async Task<Tour> CreateAsync(Tour newTour, CancellationToken ct)
    {
        newTour.Slug = Slugify(newTour.Name);

        context.Tours.Add(newTour);
        await context.SaveChangesAsync(ct);

        return newTour;
    }

    public async Task<Tour> UpdateAsync(Guid id, Tour updatedTour, CancellationToken ct)
    {
        var tour = await context.Tours.FirstOrDefaultAsync(t => t.Id.Equals(id), ct);
        if (tour is null) throw new NotFoundException($"Tour with id {id} not found.");

        if (tour.Name != updatedTour.Name)
        {
            // The tour name changed, so we need to update the slug too.
            tour.Name = updatedTour.Name;
            tour.Slug = Slugify(tour.Name);
        }
        else
        {
            tour.Name = updatedTour.Name;
        }

        tour.Duration = updatedTour.Duration;
        tour.MaxGroupSize = updatedTour.MaxGroupSize;
        tour.Difficulty = updatedTour.Difficulty;
        tour.RatingsAverage = updatedTour.RatingsAverage;
        tour.RatingsQuantity = updatedTour.RatingsQuantity;
        tour.Price = updatedTour.Price;
        tour.PriceDiscount = updatedTour.PriceDiscount;
        tour.Summary = updatedTour.Summary;
        tour.Description = updatedTour.Description;

        await context.SaveChangesAsync(ct);
        return tour;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var tourToDelete = await context.Tours.FindAsync([id], ct);
        if (tourToDelete is null) throw new NotFoundException($"Tour with id {id} not found.");

        context.Tours.Remove(tourToDelete);
        await context.SaveChangesAsync(ct);
    }

    private static string Slugify(string value)
    {
        var slug = value.ToLowerInvariant();
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
        return slug;
    }
}