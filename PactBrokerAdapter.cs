public class PactBrokerAdapter : IContractPlatform
{
    private readonly IConfiguration _config;

    public PactBrokerAdapter(
        IConfiguration config)
    {
        _config = config;
    }

    public async Task PublishPactAsync(
        string pactFolder,
        string version)
    {
        var cmd =
            $"pact-broker publish {pactFolder} " +
            $"--consumer-app-version={version}";

        await Execute(cmd);
    }

    public async Task<CanDeployDecision>
        CanIDeployAsync(
            string consumerVersion,
            string providerVersion)
    {
        var cmd =
            $"pact-broker can-i-deploy " +
            $"--version={providerVersion}";

        var output = await Execute(cmd);

        return Parse(output);
    }

    public Task PublishVerificationResultsAsync(
        VerificationResult result)
    {
        return Task.CompletedTask;
    }

    public Task<ContractMatrix>
        GetVerificationMatrixAsync()
    {
        return Task.FromResult(
            new ContractMatrix());
    }

    private Task<string> Execute(
        string command)
    {
        return Task.FromResult("");
    }

    private CanDeployDecision Parse(
        string output)
    {
        return new CanDeployDecision
        {
            CanIDeploy = true,
            Decision = "SAFE_TO_DEPLOY"
        };
    }
}
