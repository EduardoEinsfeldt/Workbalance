using Microsoft.EntityFrameworkCore;
using Workbalance.Domain.Entity;

namespace Workbalance.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DBSets
        public DbSet<User> Users { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder
                .Entity<MoodEntry>()
                .Property(m => m.Date)
                .HasConversion<DateOnlyConverter, DateOnlyComparer>()
                .HasColumnType("DATE");

            base.OnModelCreating(modelBuilder);
        }
    }

    public class DateOnlyConverter : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
            : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }

    public class DateOnlyComparer : Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<DateOnly>
    {
        public DateOnlyComparer() : base(
            (d1, d2) => d1.Year == d2.Year && d1.DayOfYear == d2.DayOfYear,
            d => d.GetHashCode())
        { }
    }
}
