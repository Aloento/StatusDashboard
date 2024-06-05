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
            Console.WriteLine(item?.Name);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        Console.WriteLine("Stop");
    }
}
