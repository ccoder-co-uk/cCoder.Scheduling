using cCoder.Scheduling.Services.Orchestrations;
using Microsoft.Extensions.Hosting;


namespace cCoder.Scheduling.Exposures.HostedServices;

public sealed class TaskRunnerHostedService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<TaskRunnerHostedService> log)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (int.TryParse(Environment.GetEnvironmentVariable("MIGRATING"), out int result) && result == 1)
            return;

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using IServiceScope scope = serviceScopeFactory.CreateScope();
                ITaskRunnerOrchestrationService orchestrationService =
                    scope.ServiceProvider.GetRequiredService<ITaskRunnerOrchestrationService>();
                await orchestrationService.RunAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
            }
        }
    }
}
