using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskProcessingServiceTests
{
    [Fact]
    public async Task ShouldUseFoundationDeleteAsyncPerItemWhenDeleteAllAsync()
    {
        // Given
        ScheduledTask entity = CreateRandomScheduledTask();
        var id = entity.Id;
        scheduledTaskServiceMock.Setup(x => x.DeleteAsync(id)).Returns(ValueTask.CompletedTask);

        // When
        await scheduledTaskProcessingService.DeleteAllAsync(new[] { entity });

        // Then
        scheduledTaskServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
        scheduledTaskServiceMock.VerifyNoOtherCalls();
    }

}









