public record CanAIDeployDecision
{
    public string Region { get; init; } = "";

    public string Consumer { get; init; } = "";

    public string Provider { get; init; } = "";

    public string ConsumerVersion { get; init; } = "";

    public string ProviderVersion { get; init; } = "";

    public bool SchemaValidationPassed { get; init; }

    public bool BackwardCompatibilityPassed { get; init; }

    public bool BidirectionalVerificationPassed { get; init; }

    public bool ProviderVerificationPassed { get; init; }

    public bool CanIDeploy { get; init; }

    public string Decision { get; init; } = "";

    public string Reason { get; init; } = "";

    public DateTimeOffset EvaluatedAt { get; init; }
}
