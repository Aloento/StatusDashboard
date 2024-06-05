namespace StatusDashboard.Services;

using Microsoft.EntityFrameworkCore;

public class StatusContext(DbContextOptions<StatusContext> opts) : DbContext(opts) {
}
