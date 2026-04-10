using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarEntityEventProcessingServiceTests
{
    [Fact]
    public async Task ShouldPassThroughCallWhenRaiseCalendarAddEventAsync()
    {
        // Given
        Calendar entity = CreateRandomCalendar();
        calendarEntityEventServiceMock
            .Setup(x => x.RaiseCalendarAddEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseCalendarAddEventAsync(entity);

        // Then
        calendarEntityEventServiceMock.Verify(x => x.RaiseCalendarAddEventAsync(entity), Times.Once);
        calendarEntityEventServiceMock.VerifyNoOtherCalls();
    }

}









