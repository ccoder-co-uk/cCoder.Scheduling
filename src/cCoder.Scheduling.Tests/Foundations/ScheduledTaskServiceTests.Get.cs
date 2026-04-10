using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class ScheduledTaskServiceTests
{
    [Fact]
    public void ShouldDelegateToBrokerWhenGet()
    {
        // Given
        ScheduledTask scheduledTask = CreateRandomScheduledTask(id: 7);

        scheduledTaskBrokerMock.Setup(x => x.GetAllScheduledTasks(false)).Returns(new[] { scheduledTask }.AsQueryable());

        // When
        ScheduledTask result = scheduledTaskService.Get(7);

        // Then
        result.Should().BeEquivalentTo(scheduledTask);
        scheduledTaskBrokerMock.Verify(x => x.GetAllScheduledTasks(false), Times.Once);
        scheduledTaskBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<ScheduledTask>()),
            Times.AtMostOnce()
        );
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}








