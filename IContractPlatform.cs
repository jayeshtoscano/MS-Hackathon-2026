public interface IContractPlatform
{
    Task PublishPactAsync(
        string pactFolder,
        string version);

    Task<CanDeployDecision> CanIDeployAsync(
        string consumerVersion,
        string providerVersion);

    Task PublishVerificationResultsAsync(
        VerificationResult result);

    Task<ContractMatrix> GetVerificationMatrixAsync();
}
