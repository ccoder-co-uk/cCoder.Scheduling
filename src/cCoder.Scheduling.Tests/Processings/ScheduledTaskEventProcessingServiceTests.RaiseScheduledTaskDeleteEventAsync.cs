using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskEventProcessingServiceTests
{
    [Fact]
    public async Task ShouldPassThroughCallWhenRaiseScheduledTaskDeleteEventAsync()
    {
        // Given
        ScheduledTask entity = CreateRandomScheduledTask();
        scheduledTaskEventServiceMock
            .Setup(x => x.RaiseScheduledTaskDeleteEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseScheduledTaskDeleteEventAsync(entity);

        // Then
        scheduledTaskEventServiceMock.Verify(x => x.RaiseScheduledTaskDeleteEventAsync(entity), Times.Once);
        scheduledTaskEventServiceMock.VerifyNoOtherCalls();
    }

}









