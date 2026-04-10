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
    public async Task ShouldGetThenDeleteThenRaiseDeleteEventAsyncWhenDeleteAsync()
    {
        // Given
        int id = 1;
        ScheduledTask entity = CreateRandomScheduledTask();
        scheduledTaskProcessingServiceMock.Setup(x => x.GetAll(true)).Returns(new[] { entity }.AsQueryable());
        scheduledTaskProcessingServiceMock.Setup(x => x.DeleteAsync(id)).Returns(ValueTask.CompletedTask);

        scheduledTaskEventProcessingServiceMock
            .Setup(x => x.RaiseScheduledTaskDeleteEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await orchestrationService.DeleteAsync(id);

        // Then
        scheduledTaskProcessingServiceMock.Verify(x => x.GetAll(true), Times.Once);
        scheduledTaskProcessingServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
        scheduledTaskEventProcessingServiceMock.Verify(x => x.RaiseScheduledTaskDeleteEventAsync(entity), Times.Once);
    }

}









