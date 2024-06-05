namespace StatusDashboard.Services;

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

            db.Services.Add(new() {
                Name = item.Name,
                Category = dbCate,
                Region = dbRegion
            });

            foreach (var incident in item.Incidents) {

            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken) => Console.WriteLine("Stop");
}
