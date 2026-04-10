using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FizzWare.NBuilder;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Coordinations;

public partial class CalendarCoordinationServiceTests
{
    [Fact]
    public async Task ShouldFetchAndDeleteChildEventsWhenHandleCalendarDelete()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        CalendarEvent calendarEvent = Builder<CalendarEvent>
            .CreateNew()
            .With(item => item.CalendarId = calendar.Id)
            .Build();

        IQueryable<CalendarEvent> calendarEvents = new[] { calendarEvent }.AsQueryable();

        calendarEventOrchestrationServiceMock
            .Setup(service => service.GetAll(true))
            .Returns(calendarEvents);

        calendarEventOrchestrationServiceMock
            .Setup(service => service.DeleteAllAsync(It.IsAny<IEnumerable<CalendarEvent>>()))
            .Returns(ValueTask.CompletedTask);

        // When
        await coordinationService.HandleCalendarDeleteAsync(calendar);

        // Then
        calendarEventOrchestrationServiceMock.Verify(service => service.GetAll(true), Times.Once);

        calendarEventOrchestrationServiceMock.Verify(
            service => service.DeleteAllAsync(
                It.Is<IEnumerable<CalendarEvent>>(items => items.Single().CalendarId == calendar.Id)
            ),
            Times.Once
        );

        calendarEventOrchestrationServiceMock.VerifyNoOtherCalls();
    }

}








