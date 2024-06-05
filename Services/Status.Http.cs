namespace StatusDashboard.Services;

using Microsoft.Extensions.Options;

internal class StatusHttp {
    private HttpClient client { get; }

    private const string endpoint = "api/v1/component_status";

    public StatusHttp(HttpClient client, IOptions<StatusOption> config) {
        this.client = client;
        this.client.BaseAddress = new(config.Value.Source);
    }

    public IAsyncEnumerable<StatusEntity?> GetStatus(CancellationToken cancellationToken) =>
        this.client.GetFromJsonAsAsyncEnumerable<StatusEntity>(endpoint, cancellationToken);
}
