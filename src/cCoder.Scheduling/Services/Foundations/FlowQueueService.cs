using cCoder.Scheduling.Brokers.Storage;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using DataFlowDefinition = cCoder.Data.Models.Workflow.FlowDefinition;
using DataFlowInstanceData = cCoder.Data.Models.Workflow.FlowInstanceData;
using DataRole = cCoder.Data.Models.Security.Role;
using DataUser = cCoder.Data.Models.Security.User;
using DataUserRole = cCoder.Data.Models.Security.UserRole;


namespace cCoder.Scheduling.Services.Foundations;

internal partial class FlowQueueService(IFlowQueueBroker broker) : IFlowQueueService
{
    public FlowDefinition GetFlowDefinition(Guid id)
    {
        ValidateId(id);

        DataFlowDefinition dataFlowDefinition = broker.GetFlowDefinition(id);

        return dataFlowDefinition is null
            ? null
            : new FlowDefinition
            {
                Id = dataFlowDefinition.Id,
                AppId = dataFlowDefinition.AppId,
                Name = dataFlowDefinition.Name,
                Description = dataFlowDefinition.Description,
                DefinitionJson = dataFlowDefinition.DefinitionJson,
            };
    }

    public User GetUser(string id)
    {
        ValidateUserId(id);

        DataUser dataUser = broker.GetUser(id);

        return dataUser is null
            ? null
            : new User
            {
                Id = dataUser.Id,
                DisplayName = dataUser.DisplayName,
                Email = dataUser.Email,
                Roles = dataUser.Roles?.Select(ToLocalUserRole).ToArray() ?? [],
            };
    }

    public async ValueTask<FlowInstanceData> AddAsync(FlowInstanceData entity)
    {
        ValidateFlowInstanceData(entity);

        DataFlowInstanceData dataEntity = new()
        {
            Id = entity.Id,
            FlowDefinitionId = entity.FlowDefinitionId,
            State = entity.State,
            Caller = entity.Caller,
            Start = entity.Start,
            ContextString = entity.ContextString,
        };

        DataFlowInstanceData result = await broker.AddFlowInstanceDataAsync(dataEntity);

        return new FlowInstanceData
        {
            Id = result.Id,
            FlowDefinitionId = result.FlowDefinitionId,
            State = result.State,
            Caller = result.Caller,
            Start = result.Start,
            ContextString = result.ContextString,
        };
    }

    private static UserRole ToLocalUserRole(DataUserRole dataUserRole) =>
        new()
        {
            RoleId = dataUserRole.RoleId,
            UserId = dataUserRole.UserId,
            Role = dataUserRole.Role is null ? null : ToLocalRole(dataUserRole.Role),
        };

    private static Role ToLocalRole(DataRole dataRole) =>
        new()
        {
            Id = dataRole.Id,
            AppId = dataRole.AppId,
            Name = dataRole.Name,
            Description = dataRole.Description,
            Privileges = string.IsNullOrWhiteSpace(dataRole.Privs)
                ? []
                : dataRole.Privs.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
        };
}


