using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class ScheduledTaskOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldCallProcessingThenRaiseUpdateEventAsyncWhenUpdateAsync()
    {
        // Given
        ScheduledTask entity = CreateRandomScheduledTask();
        scheduledTaskProcessingServiceMock.Setup(x => x.UpdateAsync(entity)).ReturnsAsync(entity);

        scheduledTaskEventProcessingServiceMock
            .Setup(x => x.RaiseScheduledTaskUpdateEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        ScheduledTask result = await orchestrationService.UpdateAsync(entity);

        // Then
        result.Should().BeSameAs(entity);
        scheduledTaskProcessingServiceMock.Verify(x => x.UpdateAsync(entity), Times.Once);
        scheduledTaskEventProcessingServiceMock.Verify(x => x.RaiseScheduledTaskUpdateEventAsync(entity), Times.Once);
    }

}









