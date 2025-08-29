using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Data.Configurations;

public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(40);
        builder.Property(t => t.Slug).IsRequired();
        builder.Property(t => t.Summary).IsRequired();
        builder.Property(t => t.RatingsAverage).HasPrecision(3, 1);

        // Indexes
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.HasIndex(t => t.Name);
        builder.HasIndex(t => new { t.Price, t.RatingsAverage });

        // Relations
        builder.HasMany(t => t.StartDates)
            .WithOne(sd => sd.Tour)
            .HasForeignKey(sd => sd.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}