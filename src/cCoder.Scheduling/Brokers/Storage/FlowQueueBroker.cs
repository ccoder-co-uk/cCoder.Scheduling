using cCoder.Data;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Scheduling.Brokers.Storage;

public interface IFlowQueueBroker
{
    FlowDefinition GetFlowDefinition(Guid id);
    User GetUser(string id);
    ValueTask<FlowInstanceData> AddFlowInstanceDataAsync(FlowInstanceData entity);
}

internal sealed class FlowQueueBroker(ICoreContextFactory coreContextFactory) : IFlowQueueBroker
{
    public FlowDefinition GetFlowDefinition(Guid id)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();

        return coreDataContext.FlowDefinitions.FirstOrDefault(flowDefinition => flowDefinition.Id == id);
    }

    public User GetUser(string id)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();

        return coreDataContext.Users
            .Include(user => user.Roles)
                .ThenInclude(userRole => userRole.Role)
            .FirstOrDefault(user => user.Id == id);
    }

    public async ValueTask<FlowInstanceData> AddFlowInstanceDataAsync(FlowInstanceData entity)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        FlowInstanceData result = (await coreDataContext.FlowInstances.AddAsync(entity)).Entity;
        _ = await coreDataContext.SaveChangesAsync();
        return result;
    }
}


