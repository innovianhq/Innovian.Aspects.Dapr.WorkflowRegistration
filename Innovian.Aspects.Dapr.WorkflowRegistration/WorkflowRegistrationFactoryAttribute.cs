using Dapr.Workflow;
using Metalama.Framework.Advising;
using Metalama.Framework.Aspects;
using Metalama.Framework.Code.SyntaxBuilders;
using Metalama.Framework.Code;

namespace Innovian.Aspects.Dapr.WorkflowRegistration;

public sealed class WorkflowRegistrationFactoryAttribute : TypeAspect
{
    /// <summary>
    /// The names of each of the workflow types.
    /// </summary>
    public string[] WorkflowTypeNames { get; set; }

    /// <summary>
    /// The names of each of the workflow activity types.
    /// </summary>
    public string[] WorkflowActivityTypeNames { get; set; }

    public WorkflowRegistrationFactoryAttribute(string[] workflowTypeNames, string[] workflowActivityTypeNames)
    {
        WorkflowTypeNames = workflowTypeNames;
        WorkflowActivityTypeNames = workflowActivityTypeNames;
    }

    /// <inheritdoc />
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        base.BuildAspect(builder);
        
        var workflowRuntimeOptions = (INamedType)TypeFactory.GetType(typeof(WorkflowRuntimeOptions));

        builder.IntroduceMethod(nameof(BuildMethodTemplate),
            IntroductionScope.Static,
            buildMethod: b =>
            {
                b.Name = "RegisterAllEntities";
                b.Accessibility = Accessibility.Public;
                b.AddParameter("c", workflowRuntimeOptions);
                //b.AddParameter("serviceCollection", serviceCollectionType); //Need some way to specify 'this' - https://github.com/postsharp/Metalama/issues/339#issuecomment-2251105015
            }, args: new
            {
                workflowTypeNames = this.WorkflowTypeNames,
                workflowActivityTypeNames = this.WorkflowActivityTypeNames
            });
    }

    [Template]
    private void BuildMethodTemplate([CompileTime] string[] workflowTypeNames, [CompileTime] string[] workflowActivityTypeNames)
    {
        if (workflowTypeNames.Length > 0)
        {
            foreach (var workflowTypeName in workflowTypeNames.Distinct())
            {
                var exprBuilder = new ExpressionBuilder();

                var workflowType = (INamedType)TypeFactory.GetType(workflowTypeName);
                exprBuilder.AppendVerbatim("c.RegisterWorkflow<");
                exprBuilder.AppendTypeName(workflowType);
                exprBuilder.AppendVerbatim(">()");

                meta.InsertStatement(exprBuilder.ToExpression());
            }
        }

        if (workflowActivityTypeNames.Length > 0)
        {
            foreach (var activityTypeName in workflowActivityTypeNames.Distinct())
            {
                var exprBuilder = new ExpressionBuilder();

                var activityType = (INamedType)TypeFactory.GetType(activityTypeName);
                exprBuilder.AppendVerbatim("c.RegisterActivity<");
                exprBuilder.AppendTypeName(activityType);
                exprBuilder.AppendVerbatim(">()");

                meta.InsertStatement(exprBuilder.ToExpression());
            }
        }
    }
}