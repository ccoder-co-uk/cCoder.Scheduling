using cCoder.Data.Models.Planning;
using FizzWare.NBuilder;
using Moq;
using Xunit;

namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskEventProcessingServiceTests
{
    [Fact]
    public async Task ShouldRaiseScheduledTaskExecuteEventAsync()
    {
        ScheduledTask entity = Builder<ScheduledTask>.CreateNew()
            .With(x => x.Id = Random.Shared.Next(1, 10000))
            .Build();

        scheduledTaskEventServiceMock
            .Setup(x => x.RaiseScheduledTaskExecuteEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        await service.RaiseScheduledTaskExecuteEventAsync(entity);

        scheduledTaskEventServiceMock.Verify(x => x.RaiseScheduledTaskExecuteEventAsync(entity), Times.Once);
        scheduledTaskEventServiceMock.VerifyNoOtherCalls();
    }
}
