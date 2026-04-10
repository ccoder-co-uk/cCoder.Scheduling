namespace cCoder.Scheduling.Services.Orchestrations;

public interface IFlowQueueOrchestrationService
{
    ValueTask<Guid> QueueAsync(Guid id, string asUserId, string args);
}



