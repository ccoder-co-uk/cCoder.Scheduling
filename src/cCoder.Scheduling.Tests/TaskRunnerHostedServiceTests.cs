using cCoder.Scheduling.Exposures.HostedServices;
using cCoder.Scheduling.Services.Orchestrations;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cCoder.Scheduling.Tests;

public sealed class TaskRunnerHostedServiceTests
{
    [Fact]
    public async Task StartAsync_ShouldRunTaskRunnerImmediately()
    {
        using CancellationTokenSource cancellationTokenSource = new();
        Mock<ITaskRunnerOrchestrationService> orchestrationServiceMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();
        Mock<IServiceScope> serviceScopeMock = new();
        Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        Mock<IHostApplicationLifetime> applicationLifetimeMock = new();
        Mock<ILogger<TaskRunnerHostedService>> loggerMock = new();
        using CancellationTokenSource applicationStarted = new();

        applicationStarted.Cancel();

        orchestrationServiceMock
            .Setup(service => service.RunAsync(It.IsAny<CancellationToken>()))
            .Callback<CancellationToken>(_ => cancellationTokenSource.Cancel())
            .Returns(Task.CompletedTask);

        serviceProviderMock
            .Setup(provider => provider.GetService(typeof(ITaskRunnerOrchestrationService)))
            .Returns(orchestrationServiceMock.Object);

        serviceScopeMock
            .SetupGet(scope => scope.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        serviceScopeFactoryMock
            .Setup(factory => factory.CreateScope())
            .Returns(serviceScopeMock.Object);
        applicationLifetimeMock
            .SetupGet(lifetime => lifetime.ApplicationStarted)
            .Returns(applicationStarted.Token);

        TaskRunnerHostedService service = new(
            serviceScopeFactoryMock.Object,
            applicationLifetimeMock.Object,
            loggerMock.Object);

        await service.StartAsync(cancellationTokenSource.Token);
        Func<Task> stop = () => service.StopAsync(CancellationToken.None);

        await stop.Should().NotThrowAsync();

        orchestrationServiceMock.Verify(
            orchestration => orchestration.RunAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        serviceScopeFactoryMock.Verify(factory => factory.CreateScope(), Times.Once);
        serviceScopeMock.Verify(scope => scope.Dispose(), Times.Once);
    }
}
