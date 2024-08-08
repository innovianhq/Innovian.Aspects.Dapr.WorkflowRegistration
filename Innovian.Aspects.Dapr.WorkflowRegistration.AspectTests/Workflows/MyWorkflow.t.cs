using Dapr.Workflow;

namespace Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.Workflows;

public class MyWorkflow : Workflow<Guid, string>
{
    /// <summary>Override to implement workflow logic.</summary>
    /// <param name="context">The workflow context.</param>
    /// <param name="input">The deserialized workflow input.</param>
    /// <returns>The output of the workflow as a task.</returns>
    public override async Task<string> RunAsync(WorkflowContext context, Guid input)
    {
        throw new NotImplementedException();
    }
}