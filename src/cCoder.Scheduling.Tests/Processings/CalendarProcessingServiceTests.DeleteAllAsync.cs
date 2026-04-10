using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;
using DataUser = cCoder.Data.Models.Security.User;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarProcessingServiceTests
{
    [Fact]
    public async Task ShouldDeleteEachCalendarAndItsEventsWhenUserCanDeleteCalendarsForDeleteAllAsync()
    {
        // Then
        // Given
        authorizationBrokerMock.Setup(x => x.GetCurrentUser()).Returns(() => currentUser);
        Calendar calendar = CreateRandomCalendar();
        CalendarEvent calendarEvent = new()
        {
            Id = 1,
            CalendarId = calendar.Id,
            Name = "Meeting",
            Description = "Event",
            Start = DateTimeOffset.UtcNow,
            DurationInTicks = 1,
            Calendar = null!,
        };
        DataUser user = TestUsers.WithPrivilege("calendar_delete", calendar.AppId);
        currentUser = user;
        calendarServiceMock.Setup(x => x.Get(calendar.Id)).Returns(calendar);

        calendarEventServiceMock
            .Setup(x => x.GetAll())
            .Returns(new[] { calendarEvent }.AsQueryable());

        calendarEventServiceMock
            .Setup(x =>
                x.DeleteAllAsync(
                    It.Is<IEnumerable<CalendarEvent>>(items =>
                        items.Single().Id == calendarEvent.Id
                    )
                )
            )
            .Returns(ValueTask.CompletedTask);

        calendarServiceMock.Setup(x => x.DeleteAsync(calendar.Id)).Returns(ValueTask.CompletedTask);

        // When
        await calendarProcessingService.DeleteAllAsync(new[] { calendar });

        // Then
        calendarServiceMock.Verify(x => x.Get(calendar.Id), Times.Once);
        calendarServiceMock.Verify(x => x.DeleteAsync(calendar.Id), Times.Once);
        calendarServiceMock.VerifyNoOtherCalls();
        calendarEventServiceMock.Verify(x => x.GetAll(), Times.Once);
        calendarEventServiceMock.Verify(
            x =>
                x.DeleteAllAsync(
                    It.Is<IEnumerable<CalendarEvent>>(items =>
                        items.Single().Id == calendarEvent.Id
                    )
                ),
            Times.Once
        );
        calendarEventServiceMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize(calendar.AppId, "calendar_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}











