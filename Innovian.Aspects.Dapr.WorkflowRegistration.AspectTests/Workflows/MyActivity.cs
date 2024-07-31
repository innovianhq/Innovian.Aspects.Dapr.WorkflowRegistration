using Dapr.Workflow;

namespace Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.Workflows;

public class MyActivity : WorkflowActivity<Guid, string>
{
    /// <summary>
    /// Override to implement async (non-blocking) workflow activity logic.
    /// </summary>
    /// <param name="context">Provides access to additional context for the current activity execution.</param>
    /// <param name="input">The deserialized activity input.</param>
    /// <returns>The output of the activity as a task.</returns>
    public override async Task<string> RunAsync(WorkflowActivityContext context, Guid input)
    {
        throw new NotImplementedException();
    }
}