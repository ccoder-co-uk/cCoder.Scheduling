using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarEventOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldGetThenDeleteThenRaiseDeleteEventAsyncWhenDeleteAsync()
    {
        // Given
        int id = 1;
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventProcessingServiceMock.Setup(x => x.GetAll(true)).Returns(new[] { entity }.AsQueryable());
        calendarEventProcessingServiceMock.Setup(x => x.DeleteAsync(id)).Returns(ValueTask.CompletedTask);

        calendarEventEventProcessingServiceMock
            .Setup(x => x.RaiseCalendarEventDeleteEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await orchestrationService.DeleteAsync(id);

        // Then
        calendarEventProcessingServiceMock.Verify(x => x.GetAll(true), Times.Once);
        calendarEventProcessingServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
        calendarEventEventProcessingServiceMock.Verify(x => x.RaiseCalendarEventDeleteEventAsync(entity), Times.Once);
    }

}









