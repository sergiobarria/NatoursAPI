using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Data.Seeders;

public static class ToursSeeder
{
    public static void Seed(DbContext dbContext)
    {
        SeedAsync(dbContext, CancellationToken.None).GetAwaiter().GetResult();
    }

    public static async Task SeedAsync(DbContext dbContext, CancellationToken ct)
    {
        var logger = dbContext.GetService<ILoggerFactory>().CreateLogger("ToursSeeder");

        try
        {
            var db = dbContext.Set<Tour>();

            if (await db.AnyAsync(ct))
            {
                logger.LogInformation("TourSeeder: there are already tours in the database.");
                return;
            }

            var baseDirectory = AppContext.BaseDirectory;
            var jsonPath = Path.Combine(baseDirectory, "Seeders", "Data", "tours-simple.json");
            if (!File.Exists(jsonPath))
            {
                logger.LogWarning("ToursSeeder: no file found at {Path}", jsonPath);
                return;
            }

            var json = await File.ReadAllTextAsync(jsonPath, ct);
            var items = JsonSerializer.Deserialize<List<TourSeedDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? [];

            var nowUtc = DateTime.UtcNow;
            var tours = new List<Tour>();
            var startDates = new List<TourStartDate>();
            var random = new Random();

            foreach (var item in items)
            {
                logger.LogInformation("TourSeeder: Adding tour {TourName}", item.Name);
                var id = Guid.CreateVersion7();
                var name = item.Name.Trim();

                var tour = new Tour
                {
                    Id = id,
                    Name = name,
                    Slug = Slugify(name),
                    Duration = item.Duration,
                    MaxGroupSize = item.MaxGroupSize,
                    Difficulty = MapDifficulty(item.Difficulty),
                    RatingsAverage = (decimal)item.RatingAvg,
                    RatingsQuantity = item.RatingsQty,
                    Price = (decimal)item.Price,
                    Summary = item.Summary,
                    Description = item.Description,
                    CreatedAt = nowUtc,
                    UpdatedAt = nowUtc
                };

                logger.LogInformation("ToursSeeder: Entity {@Tour}", tour);

                tours.Add(tour);

                // Generate tour dates
                var generated = GenerateStartDatesUtc(random, 3, 5, 365);
                startDates.AddRange(generated.Select(d => new TourStartDate
                {
                    Id = Guid.CreateVersion7(),
                    TourId = id,
                    Date = d
                }));
            }

            await dbContext.Set<Tour>().AddRangeAsync(tours, ct);
            await dbContext.Set<TourStartDate>().AddRangeAsync(startDates, ct);
            await dbContext.SaveChangesAsync(ct);

            logger.LogInformation("ToursSeeder: added {Tours} tours and {Dates} dates", tours.Count, startDates.Count);
        }
        catch (Exception e)
        {
            logger.LogError(e, "ToursSeeder: something went wrong.");
            throw;
        }
    }

    private static List<DateTime> GenerateStartDatesUtc(Random random, int minCount, int maxCount, int withinDays)
    {
        var count = random.Next(minCount, maxCount + 1);
        var set = new HashSet<DateTime>();

        for (var k = 0; k < 30 && set.Count < count; k++)
        {
            var daysAhead = random.Next(3, withinDays + 1);
            var date = DateTime.UtcNow.AddDays(daysAhead);

            var hour = new[] { 9, 13, 17 }[random.Next(0, 3)];
            var dt = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0, DateTimeKind.Utc);

            set.Add(dt);
        }

        return set.OrderBy(d => d).ToList();
    }

    private static Difficulty MapDifficulty(string? value)
    {
        return value?.Trim().ToLowerInvariant() switch
        {
            "easy" => Difficulty.Easy,
            "moderate" => Difficulty.Medium,
            "difficult" => Difficulty.Difficult,
            _ => Difficulty.Medium
        };
    }

    private static string Slugify(string value)
    {
        var slug = value.ToLowerInvariant();
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
        return slug;
    }

    private sealed record TourSeedDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("duration")]
        int Duration,
        [property: JsonPropertyName("max_group_size")]
        int MaxGroupSize,
        [property: JsonPropertyName("difficulty")]
        string Difficulty,
        [property: JsonPropertyName("ratings_avg")]
        double RatingAvg,
        [property: JsonPropertyName("ratings_qty")]
        int RatingsQty,
        [property: JsonPropertyName("price")] double Price,
        [property: JsonPropertyName("summary")]
        string Summary,
        [property: JsonPropertyName("description")]
        string? Description,
        [property: JsonPropertyName("start_dates")]
        List<DateTime>? StartDates
    );
}