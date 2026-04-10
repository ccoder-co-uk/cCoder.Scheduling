using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Coordinations;

public partial class CalendarCoordinationServiceTests
{
    [Fact]
    public async Task ShouldAddOrUpdateChildEventsWhenHandleCalendarUpdate()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        calendarEventOrchestrationServiceMock
            .Setup(service => service.AddOrUpdate(calendar.Events))
            .ReturnsAsync([]);

        // When
        await coordinationService.HandleCalendarUpdateAsync(calendar);

        // Then
        calendarEventOrchestrationServiceMock.Verify(
            service => service.AddOrUpdate(calendar.Events),
            Times.Once
        );

        calendarEventOrchestrationServiceMock.VerifyNoOtherCalls();
    }

}








