using System.Net.Http.Json;

namespace ContractAgents;

public class ContractVerificationAgent
{
    private readonly HttpClient _http;

    public ContractVerificationAgent(HttpClient http)
    {
        _http = http;
    }

    public async Task<VerificationResult> VerifyAsync(
        string consumer,
        string provider,
        string region)
    {
        await CallMcpTool(
            "run_consumer_tests",
            new
            {
                consumerName = consumer,
                region
            });

        await CallMcpTool(
            "generate_pacts",
            new
            {
                consumerName = consumer,
                region
            });

        await CallMcpTool(
            "validate_openapi_schema",
            new
            {
                provider
            });

        await CallMcpTool(
            "compare_openapi_versions",
            new
            {
                provider
            });

        var verification =
            await CallMcpTool(
                "verify_provider",
                new
                {
                    providerName = provider,
                    region
                });

        return new VerificationResult
        {
            Consumer = consumer,
            Provider = provider,
            Region = region,
            VerificationPassed = verification.Success
        };
    }

    private async Task<McpResponse> CallMcpTool(
        string tool,
        object payload)
    {
        var response =
            await _http.PostAsJsonAsync(
                $"mcp/tools/{tool}",
                payload);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<McpResponse>()
            ?? new McpResponse();
    }
}

public record VerificationResult
{
    public string Consumer { get; init; } = "";
    public string Provider { get; init; } = "";
    public string Region { get; init; } = "";
    public bool VerificationPassed { get; init; }
}

public record McpResponse
{
    public bool Success { get; init; }
}
