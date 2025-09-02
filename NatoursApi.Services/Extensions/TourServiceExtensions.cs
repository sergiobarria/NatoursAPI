using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using NatoursApi.Domain.Entities;
using NatoursApi.Services.Extensions.Utility;

namespace NatoursApi.Services.Extensions;

public static class TourServiceExtensions
{
    public static IQueryable<Tour> FilterByName(this IQueryable<Tour> tours, string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return tours;

        var lowerCaseName = name.Trim().ToLower();
        return tours.Where(t => t.Name.ToLower().Contains(lowerCaseName));
    }

    public static IQueryable<Tour> FilterBySlug(this IQueryable<Tour> tours, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return tours;

        var lowerCaseSlug = slug.Trim().ToLower();
        return tours.Where(t => t.Slug == lowerCaseSlug);
    }

    public static IQueryable<Tour> FilterByPriceRange(this IQueryable<Tour> tours, decimal minPrice, decimal maxPrice)
    {
        return tours.Where(t => t.Price >= minPrice && t.Price <= maxPrice);
    }

    public static IQueryable<Tour> FilterByDurationRange(this IQueryable<Tour> tours, uint minDuration,
        uint maxDuration)
    {
        return tours.Where(t => t.Duration >= minDuration && t.Duration <= maxDuration);
    }

    public static IQueryable<Tour> FilterByMaxGroupSize(this IQueryable<Tour> tours, uint minGroupSize,
        uint maxGroupSize)
    {
        return tours.Where(t => t.MaxGroupSize >= minGroupSize && t.MaxGroupSize <= maxGroupSize);
    }

    public static IQueryable<Tour> Sort(this IQueryable<Tour> tours, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString)) return tours;

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Tour>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery)) return tours.OrderBy(t => t.Name);

        return tours.OrderBy(orderQuery);
    }

    public static IQueryable<Tour> IncludeRelations(this IQueryable<Tour> tours, string includes)
    {
        if (string.IsNullOrWhiteSpace(includes)) return tours;

        var query = tours;
        var includeParams = includes.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

        var availableIncludes =
            new Dictionary<string, Func<IQueryable<Tour>, IQueryable<Tour>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["startDates"] = q => q.Include(t => t.StartDates)
                // Add other relations here...
            };

        foreach (var param in includeParams)
            if (availableIncludes.TryGetValue(param.Trim(), out var includeFunc))
                query = includeFunc(query);

        return query;
    }
}