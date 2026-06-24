using cCoder.Scheduling.Services.Orchestrations;
using Microsoft.Extensions.Hosting;


namespace cCoder.Scheduling.Exposures.HostedServices;

public sealed class TaskRunnerHostedService(
    IServiceScopeFactory serviceScopeFactory,
    IHostApplicationLifetime applicationLifetime,
    ILogger<TaskRunnerHostedService> log)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (int.TryParse(Environment.GetEnvironmentVariable("MIGRATING"), out int result) && result == 1)
            return;

        log.LogDebug("Task runner waiting for application startup.");
        await WaitForApplicationStartedAsync(stoppingToken);
        log.LogInformation("Task runner started.");
        await RunPendingTasksAsync(stoppingToken);

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            await RunPendingTasksAsync(stoppingToken);
    }

    private async Task RunPendingTasksAsync(CancellationToken stoppingToken)
    {
        try
        {
            log.LogDebug("Task runner checking for due tasks.");
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

    private async Task WaitForApplicationStartedAsync(CancellationToken stoppingToken)
    {
        if (applicationLifetime.ApplicationStarted.IsCancellationRequested)
            return;

        TaskCompletionSource started = new(TaskCreationOptions.RunContinuationsAsynchronously);

        using CancellationTokenRegistration startedRegistration =
            applicationLifetime.ApplicationStarted.Register(static state =>
                ((TaskCompletionSource)state!).TrySetResult(), started);
        using CancellationTokenRegistration stoppingRegistration =
            stoppingToken.Register(static state =>
                ((TaskCompletionSource)state!).TrySetCanceled(), started);

        await started.Task;
    }
}
