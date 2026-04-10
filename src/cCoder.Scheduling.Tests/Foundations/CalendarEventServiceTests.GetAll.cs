using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarEventServiceTests
{
    [Fact]
    public void ShouldDelegateToBrokerWhenGetAll()
    {
        // Given
        cCoder.Data.Models.Planning.CalendarEvent calendarEvent = CreateRandomCalendarEvent();
        IQueryable<cCoder.Data.Models.Planning.CalendarEvent> calendarEvents = new[]
        {
            calendarEvent
        }.AsQueryable();

        calendarEventBrokerMock.Setup(x => x.GetAllCalendarEvents(false)).Returns(calendarEvents);

        // When
        IQueryable<CalendarEvent> result = calendarEventService.GetAll();

        // Then
        result.Should().BeEquivalentTo(calendarEvents.Select(item => (CalendarEvent)item));
        calendarEventBrokerMock.Verify(x => x.GetAllCalendarEvents(false), Times.Once);
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<cCoder.Data.Models.Planning.CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}









