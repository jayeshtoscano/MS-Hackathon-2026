public static class ContractPlatformFactory
{
    public static IContractPlatform Create(
        IServiceProvider services,
        IConfiguration config)
    {
        var type =
            config["ContractPlatform:Type"];

        return type switch
        {
            "PactFlow" =>
                services.GetRequiredService<
                    PactFlowAdapter>(),

            _ =>
                services.GetRequiredService<
                    PactBrokerAdapter>()
        };
    }
}
