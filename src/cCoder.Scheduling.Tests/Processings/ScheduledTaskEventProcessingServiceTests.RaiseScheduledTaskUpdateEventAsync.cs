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
    public async Task ShouldPassThroughCallWhenRaiseScheduledTaskUpdateEventAsync()
    {
        // Given
        ScheduledTask entity = CreateRandomScheduledTask();
        scheduledTaskEventServiceMock
            .Setup(x => x.RaiseScheduledTaskUpdateEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseScheduledTaskUpdateEventAsync(entity);

        // Then
        scheduledTaskEventServiceMock.Verify(x => x.RaiseScheduledTaskUpdateEventAsync(entity), Times.Once);
        scheduledTaskEventServiceMock.VerifyNoOtherCalls();
    }

}









