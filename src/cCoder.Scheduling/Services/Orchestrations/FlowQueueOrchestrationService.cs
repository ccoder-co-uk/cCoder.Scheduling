using System.Security;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Foundations;
using cCoder.Workflow.Activities;
using IJsonBroker = cCoder.Scheduling.Brokers.IJsonBroker;
using cCoder.Workflow.Activities.Models;


namespace cCoder.Scheduling.Services.Orchestrations;

internal partial class FlowQueueOrchestrationService(IFlowQueueService flowQueueService, IJsonBroker jsonBroker)
    : IFlowQueueOrchestrationService
{
    public async ValueTask<Guid> QueueAsync(Guid id, string asUserId, string args)
    {
        ValidateId(id);
        ValidateUserId(asUserId);
        ValidateArgs(args);

        FlowDefinition flowDefinition = flowQueueService.GetFlowDefinition(id);
        User asUser = flowQueueService.GetUser(asUserId);
        FlowInstanceData flowInstance = CreateFlowInstanceData(flowDefinition, asUser, args, jsonBroker);
        FlowInstanceData result = await flowQueueService.AddAsync(flowInstance);
        return result.Id;
    }

    private static FlowInstanceData CreateFlowInstanceData(
        FlowDefinition flowDefinition,
        User asUser,
        string args,
        IJsonBroker jsonBroker
    )
    {
        if (flowDefinition == null)
            throw new SecurityException("Access Denied!");

        if (!HasExecutionAccess(asUser, flowDefinition.AppId))
            throw new SecurityException("Access Denied!");

        Guid instanceId = Guid.NewGuid();

        WorkflowContext context = new()
        {
            ExecutionState = "Queued",
            InstanceId = instanceId,
            Flow = ParseFlow(flowDefinition.DefinitionJson, jsonBroker),
            Variables = new Dictionary<string, object> { { "Data", args } },
            ExecutionLog = Array.Empty<WorkflowLogEntry>(),
        };

        ((Start)context.Flow.Activities.First(f => f is Start)).Data = jsonBroker.ParseJson(args);

        return new FlowInstanceData
        {
            Id = instanceId,
            State = "Queued",
            FlowDefinitionId = flowDefinition.Id,
            Start = DateTimeOffset.UtcNow,
            Caller = asUser.Id,
            ContextString = jsonBroker.Serialize(context),
        };
    }

    private static bool HasExecutionAccess(User asUser, int appId)
    {
        if (asUser?.Roles is null)
            return false;

        return asUser.Roles.Any(userRole =>
            userRole.Role is not null
            && userRole.Role.AppId == appId
            && (
                userRole.Role.Privileges.Contains("app_admin", StringComparer.OrdinalIgnoreCase)
                || userRole.Role.Privileges.Contains("flowdefinition_execute", StringComparer.OrdinalIgnoreCase)
            ));
    }

    private static Flow ParseFlow(string definitionJson, IJsonBroker jsonBroker) =>
        !string.IsNullOrWhiteSpace(definitionJson)
            ? jsonBroker.ParseJson<Flow>(definitionJson)
            : null;
}


