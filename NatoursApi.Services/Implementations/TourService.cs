using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NatoursApi.Data;
using NatoursApi.Domain.Entities;
using NatoursApi.Domain.Exceptions;
using NatoursApi.Services.Abstractions;
using NatoursApi.Services.Extensions;
using Shared.RequestFeatures;

namespace NatoursApi.Services.Implementations;

public class TourService(ApplicationDbContext context) : ITourService
{
    public async Task<PagedList<Tour>> GetAllAsync(TourQueryParameters queryParameters, CancellationToken ct)
    {
        var query = context.Tours.AsNoTracking()
            .IncludeRelations(queryParameters.Includes)
            .FilterByName(queryParameters.Name)
            .FilterBySlug(queryParameters.Slug)
            .FilterByPriceRange(queryParameters.MinPrice, queryParameters.MaxPrice)
            .FilterByMaxGroupSize(queryParameters.MinGroupSize, queryParameters.MaxGroupSize)
            .Sort(queryParameters.OrderBy!);

        var tours = await query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync(ct);

        var totalItemCount = await query.CountAsync(ct);

        var metadata = new PaginationMetadata(totalItemCount, queryParameters.PageNumber, queryParameters.PageSize);

        return new PagedList<Tour>
        {
            Items = tours,
            Metadata = metadata
        };
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
        var tour = await GetTourAndCheckIfExists(id, ct);

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
        var tourToDelete = await GetTourAndCheckIfExists(id, ct);

        context.Tours.Remove(tourToDelete);
        await context.SaveChangesAsync(ct);
    }

    private async Task<Tour> GetTourAndCheckIfExists(Guid id, CancellationToken ct)
    {
        var tour = await context.Tours.FirstOrDefaultAsync(t => t.Id.Equals(id), ct);
        if (tour is null) throw new NotFoundException($"Tour with id {id} not found.");

        return tour;
    }

    private static string Slugify(string value)
    {
        var slug = value.ToLowerInvariant();
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
        return slug;
    }
}