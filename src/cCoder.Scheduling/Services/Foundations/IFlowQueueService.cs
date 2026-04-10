using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;


namespace cCoder.Scheduling.Services.Foundations;

public interface IFlowQueueService
{
    FlowDefinition GetFlowDefinition(Guid id);
    User GetUser(string id);
    ValueTask<FlowInstanceData> AddAsync(FlowInstanceData entity);
}

