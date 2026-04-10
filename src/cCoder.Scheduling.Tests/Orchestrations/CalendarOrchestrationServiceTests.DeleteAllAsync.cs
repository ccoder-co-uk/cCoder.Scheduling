using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldDelegateToProcessingServiceWhenDeleteAllAsync()
    {
        // Given
        Calendar[] entities = [CreateRandomCalendar()];
        calendarProcessingServiceMock.Setup(x => x.DeleteAllAsync(entities)).Returns(ValueTask.CompletedTask);

        // When
        await orchestrationService.DeleteAllAsync(entities);

        // Then
        calendarProcessingServiceMock.Verify(x => x.DeleteAllAsync(entities), Times.Once);
        calendarProcessingServiceMock.VerifyNoOtherCalls();
        calendarEntityEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









