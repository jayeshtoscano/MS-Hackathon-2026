using System.Net.Http.Json;

namespace ContractAgents;

public class GatekeeperA2AAgent
{
    private readonly HttpClient _http;

    public GatekeeperA2AAgent(HttpClient http)
    {
        _http = http;
    }

    public async Task<CanDeployDecision> EvaluateAsync(
        string consumer,
        string provider,
        string consumerVersion,
        string providerVersion)
    {
        var response =
            await _http.PostAsJsonAsync(
                "mcp/tools/can_i_deploy",
                new
                {
                    consumerVersion,
                    providerVersion
                });

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<CanDeployDecision>();

        return result!;
    }

    public async Task NotifyConsumerAgent(
        CanDeployDecision decision)
    {
        await _http.PostAsJsonAsync(
            "a2a/consumer-agent",
            decision);
    }

    public async Task NotifyProviderAgent(
        CanDeployDecision decision)
    {
        await _http.PostAsJsonAsync(
            "a2a/provider-agent",
            decision);
    }
}

public record CanDeployDecision
{
    public string Region { get; init; } = "";
    public bool CanIDeploy { get; init; }
    public string Decision { get; init; } = "";
    public string Reason { get; init; } = "";
}
