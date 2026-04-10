using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class ScheduledTaskOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldDelegateToProcessingServiceWhenExecuteAsync()
    {
        scheduledTaskProcessingServiceMock
            .Setup(x => x.ExecuteAsync(1, false))
            .Returns(ValueTask.CompletedTask);

        await orchestrationService.ExecuteAsync(1, false);

        scheduledTaskProcessingServiceMock.Verify(x => x.ExecuteAsync(1, false), Times.Once);
        scheduledTaskProcessingServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}





