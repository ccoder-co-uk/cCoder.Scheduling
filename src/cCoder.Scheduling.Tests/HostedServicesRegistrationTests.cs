using cCoder.Scheduling.Exposures.HostedServices;
using cCoder.Scheduling.Services.Orchestrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;


namespace cCoder.Scheduling.Tests;

public class HostedServicesRegistrationTests
{
    [Fact]
    public void AddSchedulingHostedServices_RegistersTaskRunnerHostedService()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSchedulingHostedServices();

        Assert.Contains(
            services,
            descriptor => descriptor.ServiceType == typeof(IHostedService)
                && descriptor.ImplementationType == typeof(TaskRunnerHostedService));
        Assert.Contains(
            services,
            descriptor => descriptor.ServiceType == typeof(ITaskRunnerOrchestrationService)
                && descriptor.ImplementationType?.Name == "TaskRunnerOrchestrationService");
    }
}
