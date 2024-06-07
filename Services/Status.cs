﻿namespace StatusDashboard.Services;

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
        this.http = http;

        this.db = context.CreateDbContext();
        this.db.Database.OpenConnection();
    }

    private ILogger<StatusService> logger { get; }

    private StatusOption option { get; }

    private StatusHttp http { get; }

    private StatusContext db { get; }

    public async Task StartAsync(CancellationToken cancellationToken) {
        await this.db.Database.EnsureCreatedAsync(cancellationToken);

        var list = this.http.GetStatus(cancellationToken);

        await foreach (var item in list) {
            if (item is null) continue;

            var targetCate = item.Attributes.Single(x => x.Name == NameEnum.Category).Value;

            var dbCate = await this.db.Categories
                             .Where(x => x.Name == targetCate)
                             .SingleOrDefaultAsync(cancellationToken)
                         ?? this.db.Categories.Add(new() {
                             Name = targetCate,
                             Abbr = item.Attributes.Single(x => x.Name == NameEnum.Type).Value
                         }).Entity;

            var targetRegion = item.Attributes.Single(x => x.Name == NameEnum.Region).Value;

            var dbRegion = await this.db.Regions
                               .Where(x => x.Name == targetRegion)
                               .SingleOrDefaultAsync(cancellationToken)
                           ?? this.db.Regions.Add(new() {
                               Name = targetRegion
                           }).Entity;

            var targetService = item.Name;

            var dbService = await this.db.Services
                                .Where(x => x.Name == targetService)
                                .Include(x => x.Regions)
                                .SingleOrDefaultAsync(cancellationToken)
                            ?? this.db.Services.Add(new() {
                                Name = targetService,
                                Category = dbCate,
                                Regions = [dbRegion]
                            }).Entity;

            if (dbService.Regions.All(x => x.Name != targetRegion))
                dbService.Regions.Add(dbRegion);

            await this.db.SaveChangesAsync(cancellationToken);

            var regionService = await this.db.RegionService
                .Where(x => x.Region == dbRegion)
                .Where(x => x.Service == dbService)
                .Include(x => x.Events)
                .SingleAsync(cancellationToken);

            foreach (var incident in item.Incidents) {
                var dbEvent = await this.db.Events
                    .Where(x => x.Id == incident.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                if (dbEvent is null) {
                    dbEvent = this.db.Events.Add(new() {
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
                        var history = this.db.Histories.Add(new() {
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

                regionService.Events.Add(dbEvent);
                await this.db.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        this.http.Dispose();
        await this.db.DisposeAsync();
    }
}
