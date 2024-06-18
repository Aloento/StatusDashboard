namespace StatusDashboard.Services;

using Components.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

internal class SlaService(IDbContextFactory<StatusContext> factory, IMemoryCache cache) : IAsyncDisposable {
    private StatusContext db { get; } = factory.CreateDbContext();

    private IMemoryCache cache { get; } = cache;

    public async Task<List<double>> Calc6Months(RegionService service) {
        var cacheKey = $"{nameof(this.Calc6Months)}_{service.Id}";
        if (this.cache.TryGetValue(cacheKey, out List<double>? res)) 
            return res!;

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
            var daysInMonth = DateTime.DaysInMonth(startOfMonth.Year, startOfMonth.Month);
            var endOfMonth = new DateTime(startOfMonth.Year, startOfMonth.Month, daysInMonth);

            var monthlyEvents = events
                .Where(e => e.Start < endOfMonth)
                .Where(e => (e.End ?? now) > startOfMonth);

            var totalDowntime =
                (from evt in monthlyEvents
                    let start = evt.Start < startOfMonth
                        ? startOfMonth
                        : evt.Start
                    let end = (evt.End.HasValue && evt.End.Value < endOfMonth)
                        ? evt.End.Value
                        : endOfMonth
                    select (end - start).TotalMinutes)
                .Sum();

            var totalMinutes = (endOfMonth - startOfMonth).TotalMinutes;
            var uptimePercentage = (totalMinutes - totalDowntime) / totalMinutes * 100;
            results.Add(Math.Max(uptimePercentage, 0));
        }

        results.Reverse();
        this.cache.Set(cacheKey, results, TimeSpan.FromHours(1));
        return results;
    }

    public async ValueTask DisposeAsync() => await this.db.DisposeAsync();
}
