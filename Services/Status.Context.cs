namespace StatusDashboard.Services;

using Microsoft.EntityFrameworkCore;

internal class StatusContext(DbContextOptions<StatusContext> opts) : DbContext(opts) {
    public DbSet<Service> Services { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<History> Histories { get; set; }

    public DbSet<RegionService> RegionService { get; set; }

    public DbSet<EventRegionService> EventRegionService { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Service>()
            .HasMany(s => s.Regions)
            .WithMany(r => r.Services)
            .UsingEntity<RegionService>();

        modelBuilder.Entity<Event>()
            .HasMany(e => e.RegionServices)
            .WithMany(rs => rs.Events)
            .UsingEntity<EventRegionService>();
    }
}
