using Microsoft.EntityFrameworkCore;
using NatoursApi.Data.Seeders;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Tour> Tours => Set<Tour>();

    public DbSet<TourStartDate> TourStartDates => Set<TourStartDate>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // This will read all the configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) => ToursSeeder.Seed(context))
            .UseAsyncSeeding((context, _, ct) => ToursSeeder.SeedAsync(context, ct));
    }
}