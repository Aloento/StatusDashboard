namespace StatusDashboard.Services;

using Components.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

internal class StatusService : IHostedService {
    public StatusService(
        ILogger<StatusService> logger,
        IOptions<StatusOption> config,
        IDbContextFactory<StatusContext> context,
        StatusHttp http) {
        this.logger = logger;
        this.option = config.Value;
        this.context = context;
        this.http = http;
    }

    private ILogger<StatusService> logger { get; }

    private StatusOption option { get; }

    private StatusHttp http { get; }

    private IDbContextFactory<StatusContext> context { get; }

    public async Task StartAsync(CancellationToken cancellationToken) {
        await using var db = await this.context.CreateDbContextAsync(cancellationToken);
        await db.Database.EnsureCreatedAsync(cancellationToken);

        var list = this.http.GetStatus(cancellationToken);

        await foreach (var item in list) {
            if (item is null) continue;

            var targetCate = item.Attributes.Single(x => x.Name == NameEnum.Category).Value;

            var dbCate = await db.Categories
                             .Where(x => x.Name == targetCate)
                             .SingleOrDefaultAsync(cancellationToken)
                         ?? db.Categories.Add(new() {
                             Name = targetCate,
                             Abbr = item.Attributes.Single(x => x.Name == NameEnum.Type).Value
                         }).Entity;

            var targetRegion = item.Attributes.Single(x => x.Name == NameEnum.Region).Value;

            var dbRegion = await db.Regions
                               .Where(x => x.Name == targetRegion)
                               .SingleOrDefaultAsync(cancellationToken)
                           ?? db.Regions.Add(new() {
                               Name = targetRegion
                           }).Entity;

            var dbService = db.Services.Add(new() {
                Id = item.Id,
                Name = item.Name,
                Category = dbCate,
                Region = dbRegion,
                Events = new List<Event>()
            }).Entity;

            foreach (var incident in item.Incidents) {
                var dbEvent = await db.Events
                    .Where(x => x.Id == incident.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (dbEvent is null) {
                    dbEvent = db.Events.Add(new() {
                        Id = incident.Id,
                        Title = incident.Text,
                        Start = (DateTime)incident.StartDate!,
                        End = incident.EndDate
                    }).Entity;

                    dbEvent.Type = incident.Impact switch {
                        0 => EventType.Maintenance,
                        1 => EventType.MinorIssue,
                        2 => EventType.MajorIssue,
                        _ => EventType.Outage
                    };

                    foreach (var update in incident.Updates.OrderBy(x => x.Timestamp)) {
                        var history = db.Histories.Add(new() {
                            Created = (DateTime)update.Timestamp!,
                            Message = update.Text,
                            Event = dbEvent
                        }).Entity;

                        history.Status = update.Status switch {
                            StatusEnum.Analyzing => EventStatus.Investigating,
                            StatusEnum.Fixing => EventStatus.Fixing,
                            StatusEnum.Observing => EventStatus.Monitoring,
                            StatusEnum.Resolved => EventStatus.Resolved,

                            StatusEnum.Description => EventStatus.Scheduled,
                            StatusEnum.Scheduled => EventStatus.Scheduled,
                            StatusEnum.InProgress => EventStatus.Performing,
                            StatusEnum.Completed => EventStatus.Completed,

                            _ => EventStatus.SysInfo
                        };
                    }
                }

                dbService.Events.Add(dbEvent);
                await db.SaveChangesAsync(cancellationToken);
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        this.http.Dispose();
    }
}
