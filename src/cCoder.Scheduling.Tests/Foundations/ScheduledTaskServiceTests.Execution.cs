using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class ScheduledTaskServiceTests
{
    [Fact]
    public void ShouldReturnTaskForExecutionWhenGetForExecution()
    {
        ScheduledTask scheduledTask = CreateRandomScheduledTask(id: 5, appId: 7);

        scheduledTaskBrokerMock
            .Setup(x => x.GetScheduledTaskForExecution(5))
            .Returns(scheduledTask);

        cCoder.Data.Models.Planning.ScheduledTask result = scheduledTaskService.GetForExecution(5);

        result.Should().BeEquivalentTo(scheduledTask);
        scheduledTaskBrokerMock.Verify(x => x.GetScheduledTaskForExecution(5), Times.Once);
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldReturnMarkedTaskWhenMarkExecutedAsync()
    {
        ScheduledTask scheduledTask = CreateRandomScheduledTask(id: 7, appId: 9);

        scheduledTaskBrokerMock
            .Setup(x => x.MarkScheduledTaskExecutedAsync(7, true))
            .ReturnsAsync(scheduledTask);

        cCoder.Data.Models.Planning.ScheduledTask result = await scheduledTaskService.MarkExecutedAsync(7, true);

        result.Should().BeEquivalentTo(scheduledTask);
        scheduledTaskBrokerMock.Verify(x => x.MarkScheduledTaskExecutedAsync(7, true), Times.Once);
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnWhetherExecuteAsUserBelongsToApp()
    {
        scheduledTaskBrokerMock
            .Setup(x => x.ExecuteAsUserBelongsToApp("user-1", 7))
            .Returns(true);

        bool result = scheduledTaskService.ExecuteAsUserBelongsToApp("user-1", 7);

        result.Should().BeTrue();
        scheduledTaskBrokerMock.Verify(x => x.ExecuteAsUserBelongsToApp("user-1", 7), Times.Once);
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnWhetherFlowBelongsToApp()
    {
        Guid flowId = Guid.NewGuid();
        scheduledTaskBrokerMock
            .Setup(x => x.FlowBelongsToApp(flowId, 7))
            .Returns(true);

        bool result = scheduledTaskService.FlowBelongsToApp(flowId, 7);

        result.Should().BeTrue();
        scheduledTaskBrokerMock.Verify(x => x.FlowBelongsToApp(flowId, 7), Times.Once);
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }
}


