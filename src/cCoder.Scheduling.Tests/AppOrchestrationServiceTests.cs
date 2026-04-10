using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Scheduling.Services.Orchestrations;
using Moq;
using Xunit;

namespace cCoder.Scheduling.Tests;

public class AppOrchestrationServiceTests
{
    private readonly Mock<IScheduledTaskOrchestrationService> scheduledTaskOrchestrationServiceMock;
    private readonly Mock<ICalendarOrchestrationService> calendarOrchestrationServiceMock;
    private readonly AppOrchestrationService service;

    public AppOrchestrationServiceTests()
    {
        scheduledTaskOrchestrationServiceMock = new Mock<IScheduledTaskOrchestrationService>(MockBehavior.Strict);
        calendarOrchestrationServiceMock = new Mock<ICalendarOrchestrationService>(MockBehavior.Strict);
        service = new AppOrchestrationService(
            scheduledTaskOrchestrationServiceMock.Object,
            calendarOrchestrationServiceMock.Object);
    }

    [Fact]
    public async Task ShouldDeleteAppOwnedTasksAndCalendarsByAppIdWhenDeleteAsync()
    {
        scheduledTaskOrchestrationServiceMock.Setup(x => x.DeleteByAppIdAsync(5))
            .Returns(ValueTask.CompletedTask);
        calendarOrchestrationServiceMock.Setup(x => x.DeleteByAppIdAsync(5))
            .Returns(ValueTask.CompletedTask);

        await service.DeleteAsync(5);

        scheduledTaskOrchestrationServiceMock.Verify(x => x.DeleteByAppIdAsync(5), Times.Once);
        calendarOrchestrationServiceMock.Verify(x => x.DeleteByAppIdAsync(5), Times.Once);
        scheduledTaskOrchestrationServiceMock.VerifyNoOtherCalls();
        calendarOrchestrationServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldStampTaskAndCalendarAppIdsWhenAddAsync()
    {
        App app = new()
        {
            Id = 9,
            Tasks = [new ScheduledTask { Id = 1, Name = "Task" }],
            Calendars = [new Calendar { Id = 2, Name = "Calendar" }]
        };

        scheduledTaskOrchestrationServiceMock.Setup(x => x.AddOrUpdate(
                It.Is<IEnumerable<ScheduledTask>>(items => items.All(task => task.AppId == 9))))
            .Returns(ValueTask.FromResult<IEnumerable<cCoder.Scheduling.Models.Result<ScheduledTask>>>([]));
        calendarOrchestrationServiceMock.Setup(x => x.AddOrUpdate(
                It.Is<IEnumerable<Calendar>>(items => items.All(calendar => calendar.AppId == 9))))
            .Returns(ValueTask.FromResult<IEnumerable<cCoder.Scheduling.Models.Result<Calendar>>>([]));

        await service.AddAsync(app);

        scheduledTaskOrchestrationServiceMock.VerifyAll();
        calendarOrchestrationServiceMock.VerifyAll();
    }
}
