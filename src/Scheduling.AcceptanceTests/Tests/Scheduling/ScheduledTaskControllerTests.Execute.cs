using cCoder.Data.Models.Planning;
using FluentAssertions;
using Xunit;


namespace Web.AcceptanceTests.Tests.Scheduling;

public sealed partial class ScheduledTaskControllerTests
{
    [Fact]
    public async Task Execute_QueuesWorkflowInstance()
    {
        // Given
        SeededScheduledTaskContext seededContext = await SeedDatabase();
        ScheduledTask createdScheduledTask = await CreateScheduledTaskAsync(new
        {
            appId = seededContext.AppId,
            flowId = seededContext.FlowId,
            name = Unique("ScheduledTask"),
            description = "Acceptance scheduled task",
            executionArgs = "{}",
            scheduleInTicks = TimeSpan.FromHours(1).Ticks,
            executeAs = "Guest",
            createdBy = "Guest",
            updatedBy = "Guest",
            created = DateTimeOffset.UtcNow,
            lastUpdated = DateTimeOffset.UtcNow,
            nextExecution = DateTimeOffset.UtcNow.AddHours(1),
        });
        int actualStatusCode;

        // When
        actualStatusCode = await ExecuteScheduledTaskAsync(createdScheduledTask.Id, incrementNextExecution: true);

        // Then
        actualStatusCode.Should().Be(200);
        (await HasQueuedInstanceAsync(seededContext.FlowId)).Should().BeTrue();

        await DeleteScheduledTaskAsync(createdScheduledTask.Id);
        await Teardown(seededContext);
    }
}





