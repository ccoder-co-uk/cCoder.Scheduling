using cCoder.Data.Models.Planning;
using EventLibrary.Models;
using FizzWare.NBuilder;
using Moq;
using Xunit;

namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class ScheduledTaskEventServiceTests
{
    [Fact]
    public async Task ShouldRaiseScheduledTaskExecuteEventAsync()
    {
        ScheduledTask entity = Builder<ScheduledTask>.CreateNew()
            .With(x => x.Id = Random.Shared.Next(1, 10000))
            .With(x => x.ExecuteAs = CurrentUserId)
            .Build();

        scheduledTaskEventBrokerMock
            .Setup(x => x.RaiseScheduledTaskExecuteEventAsync(
                It.Is<EventMessage<cCoder.Data.Models.Planning.ScheduledTask>>(message =>
                    message.AuthInfo.SSOUserId == CurrentUserId
                    && message.Data.Id == entity.Id
                    && message.Data.ExecuteAs == entity.ExecuteAs)))
            .Returns(ValueTask.CompletedTask);

        await service.RaiseScheduledTaskExecuteEventAsync(entity);

        scheduledTaskEventBrokerMock.Verify(
            x => x.RaiseScheduledTaskExecuteEventAsync(
                It.IsAny<EventMessage<cCoder.Data.Models.Planning.ScheduledTask>>()),
            Times.Once);
        scheduledTaskEventBrokerMock.VerifyNoOtherCalls();
    }
}
