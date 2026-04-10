using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarEventOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldCallProcessingThenRaiseUpdateEventAsyncWhenUpdateAsync()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventProcessingServiceMock.Setup(x => x.UpdateAsync(entity)).ReturnsAsync(entity);

        calendarEventEventProcessingServiceMock
            .Setup(x => x.RaiseCalendarEventUpdateEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        CalendarEvent result = await orchestrationService.UpdateAsync(entity);

        // Then
        result.Should().BeSameAs(entity);
        calendarEventProcessingServiceMock.Verify(x => x.UpdateAsync(entity), Times.Once);
        calendarEventEventProcessingServiceMock.Verify(x => x.RaiseCalendarEventUpdateEventAsync(entity), Times.Once);
    }

}









