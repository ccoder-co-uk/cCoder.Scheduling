using cCoder.Data.Models.Planning;
using cCoder.Scheduling.Services.Foundations;
using cCoder.Scheduling.Services.Orchestrations;
using cCoder.Scheduling.Services.Processings;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class TaskRunnerOrchestrationServiceTests
{
    private readonly Mock<IScheduledTaskService> scheduledTaskServiceMock;
    private readonly Mock<ICalendarEventService> calendarEventServiceMock;
    private readonly Mock<IScheduledTaskEventProcessingService> scheduledTaskEventProcessingServiceMock;
    private readonly TaskRunnerOrchestrationService orchestrationService;

    public TaskRunnerOrchestrationServiceTests()
    {
        scheduledTaskServiceMock = new Mock<IScheduledTaskService>(MockBehavior.Strict);
        calendarEventServiceMock = new Mock<ICalendarEventService>(MockBehavior.Strict);
        scheduledTaskEventProcessingServiceMock = new Mock<IScheduledTaskEventProcessingService>(MockBehavior.Strict);
        orchestrationService = new TaskRunnerOrchestrationService(
            scheduledTaskServiceMock.Object,
            calendarEventServiceMock.Object,
            scheduledTaskEventProcessingServiceMock.Object,
            NullLogger<TaskRunnerOrchestrationService>.Instance
        );
    }

    private static ScheduledTask CreateDueScheduledTask(int id = 42) =>
        new()
        {
            Id = id,
            AppId = 7,
            FlowId = Guid.NewGuid(),
            Name = $"Task-{id}",
            ExecuteAs = "Guest",
            ExecutionArgs = "{}",
            ScheduleInTicks = TimeSpan.FromMinutes(5).Ticks,
            NextExecution = DateTimeOffset.UtcNow.AddMinutes(-5),
            ExecuteAsUser = new cCoder.Data.Models.Security.User
            {
                Id = "Guest",
                Roles = []
            },
            Flow = new cCoder.Data.Models.Workflow.FlowDefinition
            {
                Id = Guid.NewGuid(),
                AppId = 7
            }
        };
}
