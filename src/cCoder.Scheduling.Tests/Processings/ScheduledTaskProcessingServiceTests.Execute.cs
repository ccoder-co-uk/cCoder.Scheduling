using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskProcessingServiceTests
{
    [Fact]
    public async Task ShouldExecuteTaskWhenUserIsAppAdmin()
    {
        ScheduledTask task = CreateRandomScheduledTask();
        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(task.AppId)).Returns(true);
        scheduledTaskServiceMock.Setup(x => x.GetForExecution(task.Id)).Returns(task);
        scheduledTaskServiceMock
            .Setup(x => x.MarkExecutedAsync(task.Id, false))
            .ReturnsAsync(task);
        scheduledTaskEventProcessingServiceMock
            .Setup(x => x.RaiseScheduledTaskExecuteEventAsync(task))
            .Returns(ValueTask.CompletedTask);

        await scheduledTaskProcessingService.ExecuteAsync(task.Id, false);

        scheduledTaskServiceMock.Verify(x => x.GetForExecution(task.Id), Times.Once);
        scheduledTaskServiceMock.Verify(x => x.MarkExecutedAsync(task.Id, false), Times.Once);
        scheduledTaskEventProcessingServiceMock.Verify(
            x => x.RaiseScheduledTaskExecuteEventAsync(task),
            Times.Once
        );
        scheduledTaskServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}










