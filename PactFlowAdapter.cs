public class PactFlowAdapter : IContractPlatform
{
    private readonly HttpClient _http;

    public PactFlowAdapter(
        HttpClient http)
    {
        _http = http;
    }

    public async Task PublishPactAsync(
        string pactFolder,
        string version)
    {
        await _http.PostAsync(
            "/contracts/publish",
            BuildContent());
    }

    public async Task<CanDeployDecision>
        CanIDeployAsync(
            string consumerVersion,
            string providerVersion)
    {
        var response =
            await _http.GetFromJsonAsync<
                CanDeployDecision>(
                $"/can-i-deploy");
                
        return response!;
    }

    public async Task
        PublishVerificationResultsAsync(
            VerificationResult result)
    {
        await _http.PostAsJsonAsync(
            "/verification-results",
            result);
    }

    public async Task<ContractMatrix>
        GetVerificationMatrixAsync()
    {
        return await _http
            .GetFromJsonAsync<ContractMatrix>(
                "/matrix")
            ?? new ContractMatrix();
    }

    private MultipartFormDataContent
        BuildContent()
    {
        return new MultipartFormDataContent();
    }
}
