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
    public async Task ShouldCallProcessingThenRaiseAddEventAsyncWhenAddAsync()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventProcessingServiceMock.Setup(x => x.AddAsync(entity)).ReturnsAsync(entity);

        calendarEventEventProcessingServiceMock
            .Setup(x => x.RaiseCalendarEventAddEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        CalendarEvent result = await orchestrationService.AddAsync(entity);

        // Then
        result.Should().BeSameAs(entity);
        calendarEventProcessingServiceMock.Verify(x => x.AddAsync(entity), Times.Once);
        calendarEventEventProcessingServiceMock.Verify(x => x.RaiseCalendarEventAddEventAsync(entity), Times.Once);
    }

}









