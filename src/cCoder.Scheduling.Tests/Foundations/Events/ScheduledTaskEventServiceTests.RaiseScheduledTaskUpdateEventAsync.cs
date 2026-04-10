using cCoder.Data.Models.Planning;
using EventLibrary.Models;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class ScheduledTaskEventServiceTests
{
    [Fact]
    public async Task ShouldMapAndCallBrokerWhenRaiseScheduledTaskUpdateEventAsync()
    {
        // Given
        ScheduledTask entity = new();
        EventMessage<ScheduledTask> actualMessage = null;

        scheduledTaskEventBrokerMock
            .Setup(x =>
                x.RaiseScheduledTaskUpdateEventAsync(It.IsAny<EventMessage<ScheduledTask>>())
            )
            .Callback<EventMessage<ScheduledTask>>(message => actualMessage = message)
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseScheduledTaskUpdateEventAsync(entity);

        // Then
        actualMessage.Should().NotBeNull();
        actualMessage!.Data.Should().BeEquivalentTo(entity);
        actualMessage.AuthInfo.Should().NotBeNull();
        actualMessage.AuthInfo.SSOUserId.Should().Be(CurrentUserId);
        scheduledTaskEventBrokerMock.Verify(
            x => x.RaiseScheduledTaskUpdateEventAsync(It.IsAny<EventMessage<ScheduledTask>>()),
            Times.Once
        );
        scheduledTaskEventBrokerMock.VerifyNoOtherCalls();
    }

}









