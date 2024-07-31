using Dapr.Workflow;

namespace Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.Workflows;

public class MyOtherActivity : WorkflowActivity<bool, object?>
{
    /// <summary>
    /// Override to implement async (non-blocking) workflow activity logic.
    /// </summary>
    /// <param name="context">Provides access to additional context for the current activity execution.</param>
    /// <param name="input">The deserialized activity input.</param>
    /// <returns>The output of the activity as a task.</returns>
    public override async Task<object?> RunAsync(WorkflowActivityContext context, bool input)
    {
        throw new NotImplementedException();
    }
}