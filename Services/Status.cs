namespace StatusDashboard.Services;

using Microsoft.Extensions.Options;

public class StatusService {
    private ILogger<StatusService> logger { get; }

    private StatusOption option { get; }

    private HttpClient httpClient { get; }

    public StatusService(
        ILogger<StatusService> logger,
        IOptions<StatusOption> config,
        IHttpClientFactory clientFactory) {
        this.logger = logger;
        this.option = config.Value;

        this.httpClient = clientFactory.CreateClient();
        this.httpClient.BaseAddress = new(this.option.Source);
    }
}
