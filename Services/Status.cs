namespace StatusDashboard.Services;

using Microsoft.Extensions.Options;

public partial class StatusService : IHostedService {
    private ILogger<StatusService> logger { get; }

    private StatusOption option { get; }

    private HttpClient httpClient { get; }

    private const string endpoint = "api/v1/component_status";

    public StatusService(
        ILogger<StatusService> logger,
        IOptions<StatusOption> config,
        IHttpClientFactory clientFactory) {
        this.logger = logger;
        this.option = config.Value;

        this.httpClient = clientFactory.CreateClient();
        this.httpClient.BaseAddress = new(this.option.Source);
    }

    private async Task getStatus() {
        var list = this.httpClient.GetFromJsonAsAsyncEnumerable<StatusEntity>(endpoint);

        await foreach (var item in list) {
            Console.WriteLine(item?.Name);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        await this.getStatus();
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        Console.WriteLine("Stop");
    }
}
