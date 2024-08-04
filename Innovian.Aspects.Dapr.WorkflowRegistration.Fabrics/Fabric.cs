using Metalama.Framework.Fabrics;
using Metalama.Framework.Code;

namespace Innovian.Aspects.Dapr.WorkflowRegistration.Fabric;

public sealed class Fabric : TransitiveProjectFabric
{
    /// <summary>
    /// The user can implement this method to analyze types in the current project, add aspects, and report or suppress diagnostics.
    /// </summary>
    public override void AmendProject(IProjectAmender amender)
    {
        //Locate each of the workflows in the project
        //var workflowType = (INamedType)TypeFactory.GetType(typeof(Workflow<,>));
        var workflows = amender.SelectMany(p => p.AllTypes)
            .Where(t => t is { TypeKind: TypeKind.Class, BaseType.Name: "Workflow", BaseType.IsAbstract: true })
            .ToCollection();
        var workflowNames = workflows.Select(workflow => workflow.FullName).ToArray();

        //Locate each of the workflow activities in the project
        //var workflowActivityType = (INamedType)TypeFactory.GetType(typeof(WorkflowActivity<,>));
        var workflowActivities = amender.SelectMany(p => p.AllTypes)
            .Where(t => t is { TypeKind: TypeKind.Class, BaseType.Name: "WorkflowActivity", BaseType.IsAbstract: true })
            .ToCollection();
        var workflowActivityNames = workflowActivities.Select(activity => activity.FullName).ToArray();

        //Works
        amender.SelectMany(c => c.Types)
            .Where(t => t is {TypeKind: TypeKind.Class, Name: "Program"})
            .AddAspectIfEligible(_ => new WorkflowRegistrationFactoryAttribute(workflowNames, workflowActivityNames));
            
    }
}