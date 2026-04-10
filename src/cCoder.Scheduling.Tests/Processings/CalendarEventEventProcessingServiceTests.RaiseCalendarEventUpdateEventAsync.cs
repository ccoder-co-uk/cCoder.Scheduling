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
    public async Task ShouldPassThroughCallWhenRaiseCalendarEventUpdateEventAsync()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventEventServiceMock
            .Setup(x => x.RaiseCalendarEventUpdateEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseCalendarEventUpdateEventAsync(entity);

        // Then
        calendarEventEventServiceMock.Verify(x => x.RaiseCalendarEventUpdateEventAsync(entity), Times.Once);
        calendarEventEventServiceMock.VerifyNoOtherCalls();
    }

}









