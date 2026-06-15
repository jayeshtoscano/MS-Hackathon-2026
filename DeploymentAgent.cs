using System.Diagnostics;

namespace ContractAgents;

public class DeploymentAgent
{
    public async Task DeployAsync(
        CanDeployDecision decision)
    {
        if (!decision.CanIDeploy)
        {
            throw new InvalidOperationException(
                $"Deployment blocked: {decision.Reason}");
        }

        await ExecutePipeline();
    }

    private Task ExecutePipeline()
    {
        var process = new Process();

        process.StartInfo.FileName = "az";
        process.StartInfo.Arguments =
            "pipelines run --name AddressBook-Deploy";

        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        process.WaitForExit();

        return Task.CompletedTask;
    }
}
