public sealed class GatekeeperA2AAgent
{
    private readonly HttpClient _http;
    private readonly IContractPlatform _contractPlatform;
    private readonly ILogger<GatekeeperA2AAgent> _logger;

    public GatekeeperA2AAgent(
        HttpClient http,
        IContractPlatform contractPlatform,
        ILogger<GatekeeperA2AAgent> logger)
    {
        _http = http;
        _contractPlatform = contractPlatform;
        _logger = logger;
    }

    public async Task<CanDeployDecision> EvaluateAsync(
        string consumerVersion,
        string providerVersion,
        CancellationToken cancellationToken = default)
    {
        var decision =
            await _contractPlatform.CanIDeployAsync(
                consumerVersion,
                providerVersion);

        _logger.LogInformation(
            "Deployment decision {Decision} for Provider {ProviderVersion}",
            decision.Decision,
            providerVersion);

        return decision;
    }

    public async Task NotifyConsumerAgent(
        CanDeployDecision decision,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _http.PostAsJsonAsync(
                "a2a/consumer-agent",
                decision,
                cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task NotifyProviderAgent(
        CanDeployDecision decision,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _http.PostAsJsonAsync(
                "a2a/provider-agent",
                decision,
                cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
