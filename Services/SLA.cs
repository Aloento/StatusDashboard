namespace StatusDashboard.Services;

using Components.Event;
using Microsoft.EntityFrameworkCore;

internal class SlaService(IDbContextFactory<StatusContext> factory) : IAsyncDisposable {
    private StatusContext db { get; } = factory.CreateDbContext();

    public async Task<List<double>> Calc6Months(RegionService service) {
        var now = DateTime.UtcNow;
        var sixMonth = now.AddMonths(-6);

        var events = await this.db.RegionService
            .Where(x => x == service)
            .SelectMany(x => x.Events)
            .Where(e => e.Start >= sixMonth)
            .Where(x => x.Type != EventType.Maintenance)
            .ToArrayAsync();

        var results = new List<double>(6);

        for (var i = 0; i < 6; i++) {
            var startOfMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
            var endOfMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).AddMonths(-i);

            var monthlyEvents = events
                .Where(e => e.Start < endOfMonth)
                .Where(e => (e.End ?? now) > startOfMonth);

            double totalDowntime = 0;

            foreach (var evt in monthlyEvents) {
                var start = evt.Start < startOfMonth ? startOfMonth : evt.Start;
                var end = evt.End.HasValue && evt.End.Value < endOfMonth ? evt.End.Value : endOfMonth;
                totalDowntime += (end - start).TotalMinutes;
            }

            var totalMinutes = (endOfMonth - startOfMonth).TotalMinutes;
            var uptimePercentage = (totalMinutes - totalDowntime) / totalMinutes * 100;
            results.Add(Math.Max(uptimePercentage, 0));
        }

        results.Reverse();
        return results;
    }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();
}
