using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class ScheduledTaskServiceTests
{
    [Fact]
    public void ShouldDelegateToBrokerWhenGetAll()
    {
        // Given
        cCoder.Data.Models.Planning.ScheduledTask scheduledTask = CreateRandomScheduledTask();
        IQueryable<cCoder.Data.Models.Planning.ScheduledTask> scheduledTasks = new[]
        {
            scheduledTask
        }.AsQueryable();

        scheduledTaskBrokerMock.Setup(x => x.GetAllScheduledTasks(false)).Returns(scheduledTasks);

        // When
        IQueryable<ScheduledTask> result = scheduledTaskService.GetAll();

        // Then
        result.Should().BeEquivalentTo(scheduledTasks.Select(item => (ScheduledTask)item));
        scheduledTaskBrokerMock.Verify(x => x.GetAllScheduledTasks(false), Times.Once);
        scheduledTaskBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<cCoder.Data.Models.Planning.ScheduledTask>()),
            Times.AtMostOnce()
        );
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}









