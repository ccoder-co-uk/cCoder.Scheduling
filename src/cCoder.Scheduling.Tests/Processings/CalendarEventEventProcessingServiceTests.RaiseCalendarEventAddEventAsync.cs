using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarEventEventProcessingServiceTests
{
    [Fact]
    public async Task ShouldPassThroughCallWhenRaiseCalendarEventAddEventAsync()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventEventServiceMock
            .Setup(x => x.RaiseCalendarEventAddEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseCalendarEventAddEventAsync(entity);

        // Then
        calendarEventEventServiceMock.Verify(x => x.RaiseCalendarEventAddEventAsync(entity), Times.Once);
        calendarEventEventServiceMock.VerifyNoOtherCalls();
    }

}









