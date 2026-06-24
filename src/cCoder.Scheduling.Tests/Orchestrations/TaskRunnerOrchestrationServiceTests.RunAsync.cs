using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;

namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class TaskRunnerOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldExecuteDueTaskWhenRunAsync()
    {
        ScheduledTask dueTask = CreateDueScheduledTask(id: 1);
        ScheduledTask updatedTask = CreateDueScheduledTask(id: 1);

        scheduledTaskServiceMock
            .Setup(service => service.GetAll(true))
            .Returns(new[] { dueTask }.AsQueryable());
        calendarEventServiceMock
            .Setup(service => service.GetAll(true))
            .Returns(Array.Empty<CalendarEvent>().AsQueryable());
        scheduledTaskServiceMock
            .Setup(service => service.MarkExecutedAsync(dueTask.Id, true))
            .ReturnsAsync(updatedTask);
        scheduledTaskEventProcessingServiceMock
            .Setup(service => service.RaiseScheduledTaskExecuteEventAsync(updatedTask))
            .Returns(ValueTask.CompletedTask);

        await orchestrationService.RunAsync();

        scheduledTaskServiceMock.Verify(service => service.GetAll(true), Times.Once);
        calendarEventServiceMock.Verify(service => service.GetAll(true), Times.Once);
        scheduledTaskServiceMock.Verify(service => service.MarkExecutedAsync(dueTask.Id, true), Times.Once);
        scheduledTaskEventProcessingServiceMock.Verify(
            service => service.RaiseScheduledTaskExecuteEventAsync(updatedTask),
            Times.Once);
        scheduledTaskServiceMock.VerifyNoOtherCalls();
        calendarEventServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowClearExceptionWhenTaskCannotBeMarkedExecuted()
    {
        ScheduledTask dueTask = CreateDueScheduledTask(id: 7);

        scheduledTaskServiceMock
            .Setup(service => service.GetAll(true))
            .Returns(new[] { dueTask }.AsQueryable());
        calendarEventServiceMock
            .Setup(service => service.GetAll(true))
            .Returns(Array.Empty<CalendarEvent>().AsQueryable());
        scheduledTaskServiceMock
            .Setup(service => service.MarkExecutedAsync(dueTask.Id, true))
            .ReturnsAsync((ScheduledTask)null);

        Func<Task> action = () => orchestrationService.RunAsync();

        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage($"Scheduled task {dueTask.Id} could not be marked as executed.");

        scheduledTaskServiceMock.Verify(service => service.GetAll(true), Times.Once);
        calendarEventServiceMock.Verify(service => service.GetAll(true), Times.Once);
        scheduledTaskServiceMock.Verify(service => service.MarkExecutedAsync(dueTask.Id, true), Times.Once);
        scheduledTaskServiceMock.VerifyNoOtherCalls();
        calendarEventServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }
}
