using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NatoursApi.Domain.Entities;

public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Difficult = 2
}

public class Tour
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [StringLength(40, MinimumLength = 10)]
    public string Name { get; set; } = string.Empty;

    [Required] public string Slug { get; set; } = string.Empty;

    [Range(1, int.MaxValue)] public int Duration { get; init; }

    [Range(1, int.MaxValue)] public int MaxGroupSize { get; init; }

    [Required] public Difficulty Difficulty { get; init; }

    [Range(1, 5)] public decimal RatingsAverage { get; init; }

    [Range(0, int.MaxValue)] public int RatingsQuantity { get; init; } = 0;

    [Range(0, double.MaxValue)] public decimal Price { get; init; }

    public decimal? PriceDiscount { get; init; }

    [Required] public string Summary { get; init; } = string.Empty;

    public string? Description { get; init; }

    public ICollection<TourStartDate> StartDates { get; init; } = new List<TourStartDate>();

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public void SetName(string name)
    {
        Name = name;
        Slug = Slugify(Name);
    }

    private static string Slugify(string value)
    {
        var slug = value.ToLowerInvariant();
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
        return slug;
    }
}