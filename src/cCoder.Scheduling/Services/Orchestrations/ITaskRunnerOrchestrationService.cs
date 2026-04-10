namespace cCoder.Scheduling.Services.Orchestrations;

public interface ITaskRunnerOrchestrationService
{
    Task RunAsync(CancellationToken cancellationToken = default);
}
