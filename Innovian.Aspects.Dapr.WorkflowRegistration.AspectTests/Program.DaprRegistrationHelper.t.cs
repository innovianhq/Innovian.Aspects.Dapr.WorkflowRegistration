// @Include(Workflows/MyActivity)
// @Include(Workflows/MyOtherActivity)
// @Include(Workflows/MyWorkflow)

namespace Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.DaprUtilities
{
    public static class DaprRegistrationHelper
    {
        public static void RegisterAllEntities(global::Dapr.Workflow.WorkflowRuntimeOptions c)
        {
            c.RegisterWorkflow<global::Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.Workflows.MyWorkflow>();
            c.RegisterActivity<global::Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.Workflows.MyOtherActivity>();
            c.RegisterActivity<global::Innovian.Aspects.Dapr.WorkflowRegistration.AspectTests.Workflows.MyActivity>();
        }
    }
}