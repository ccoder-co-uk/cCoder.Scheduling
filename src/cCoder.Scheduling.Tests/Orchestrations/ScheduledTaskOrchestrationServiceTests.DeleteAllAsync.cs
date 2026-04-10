using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class ScheduledTaskOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldDelegateToProcessingServiceWhenDeleteAllAsync()
    {
        // Given
        ScheduledTask[] entities = [CreateRandomScheduledTask()];
        scheduledTaskProcessingServiceMock.Setup(x => x.DeleteAllAsync(entities)).Returns(ValueTask.CompletedTask);

        // When
        await orchestrationService.DeleteAllAsync(entities);

        // Then
        scheduledTaskProcessingServiceMock.Verify(x => x.DeleteAllAsync(entities), Times.Once);
        scheduledTaskProcessingServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









