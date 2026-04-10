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
        flowQueueOrchestrationServiceMock
            .Setup(x => x.QueueAsync(task.FlowId, task.ExecuteAs, "{}"))
            .ReturnsAsync(Guid.NewGuid());

        await scheduledTaskProcessingService.ExecuteAsync(task.Id, false);

        scheduledTaskServiceMock.Verify(x => x.GetForExecution(task.Id), Times.Once);
        scheduledTaskServiceMock.Verify(x => x.MarkExecutedAsync(task.Id, false), Times.Once);
        flowQueueOrchestrationServiceMock.Verify(
            x => x.QueueAsync(task.FlowId, task.ExecuteAs, "{}"),
            Times.Once
        );
        scheduledTaskServiceMock.VerifyNoOtherCalls();
        flowQueueOrchestrationServiceMock.VerifyNoOtherCalls();
    }

}










