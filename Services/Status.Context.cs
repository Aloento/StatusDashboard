namespace StatusDashboard.Services;

using Microsoft.EntityFrameworkCore;

internal class StatusContext(DbContextOptions<StatusContext> opts) : DbContext(opts) {
    public DbSet<Service> Services { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<History> Histories { get; set; }

    public DbSet<EventService> EventService { get; set; }
}
