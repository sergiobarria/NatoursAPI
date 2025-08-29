using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Data.Configurations;

public class TourStartDateConfiguration : IEntityTypeConfiguration<TourStartDate>
{
    public void Configure(EntityTypeBuilder<TourStartDate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired()
            .HasColumnType("timestamptz");

        builder.HasIndex(x => new { x.TourId, x.Date }).IsUnique();
    }
}