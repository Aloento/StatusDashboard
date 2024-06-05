namespace StatusDashboard.Services;

using Microsoft.Extensions.Options;

public class StatusService : IHostedService {
    private const string endpoint = "api/v1/component_status";

    public StatusService(
        ILogger<StatusService> logger,
        IOptions<StatusOption> config,
        IHttpClientFactory clientFactory,
        StatusContext context) {

        this.logger = logger;
        this.option = config.Value;
        this.context = context;

        this.httpClient = clientFactory.CreateClient();
        this.httpClient.BaseAddress = new(this.option.Source);
    }

    private ILogger<StatusService> logger { get; }

    private StatusOption option { get; }

    private HttpClient httpClient { get; }

    private StatusContext context { get; }

    public async Task StartAsync(CancellationToken cancellationToken) {
        await this.context.Database.EnsureCreatedAsync(cancellationToken);
        await this.getStatus(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        Console.WriteLine("Stop");
    }

    private async Task getStatus(CancellationToken cancellationToken) {
        var list = this.httpClient.GetFromJsonAsAsyncEnumerable<StatusEntity>(endpoint, cancellationToken);

        await foreach (var item in list)
            Console.WriteLine(item?.Name);
    }
}
