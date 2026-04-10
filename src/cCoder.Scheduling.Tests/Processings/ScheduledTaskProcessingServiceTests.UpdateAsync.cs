using System.Security;
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
    public async Task ShouldDelegateToFoundationServiceWhenSecurityChecksPassForUpdateAsync()
    {
        // Given
        ScheduledTask task = CreateRandomScheduledTask();
        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(task.AppId)).Returns(true);
        scheduledTaskServiceMock
            .Setup(x => x.ExecuteAsUserBelongsToApp(task.ExecuteAs, task.AppId))
            .Returns(true);
        scheduledTaskServiceMock
            .Setup(x => x.FlowBelongsToApp(task.FlowId, task.AppId))
            .Returns(true);
        scheduledTaskServiceMock.Setup(x => x.UpdateAsync(task)).ReturnsAsync(task);

        // When
        ScheduledTask result = await scheduledTaskProcessingService.UpdateAsync(task);

        // Then
        Assert.Same(task, result);
        scheduledTaskServiceMock.Verify(
            x => x.ExecuteAsUserBelongsToApp(task.ExecuteAs, task.AppId),
            Times.Once
        );
        scheduledTaskServiceMock.Verify(x => x.FlowBelongsToApp(task.FlowId, task.AppId), Times.Once);
        scheduledTaskServiceMock.Verify(x => x.UpdateAsync(task), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenFlowBelongsToDifferentAppForUpdateAsync()
    {
        // Given
        ScheduledTask task = CreateRandomScheduledTask();
        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(task.AppId)).Returns(true);
        scheduledTaskServiceMock
            .Setup(x => x.ExecuteAsUserBelongsToApp(task.ExecuteAs, task.AppId))
            .Returns(true);
        scheduledTaskServiceMock
            .Setup(x => x.FlowBelongsToApp(task.FlowId, task.AppId))
            .Returns(false);

        // When
        await Assert.ThrowsAsync<SecurityException>(async () =>
            await scheduledTaskProcessingService.UpdateAsync(task)
        );

        // Then
    }

}









