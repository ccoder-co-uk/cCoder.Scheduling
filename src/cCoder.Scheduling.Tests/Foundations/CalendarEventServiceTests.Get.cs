using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarEventServiceTests
{
    [Fact]
    public void ShouldDelegateToBrokerWhenGet()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        calendarEventBrokerMock
            .Setup(x => x.GetAllCalendarEvents(false))
            .Returns(new[] { calendarEvent }.AsQueryable());

        // When
        CalendarEvent result = calendarEventService.Get(calendarEvent.Id);

        // Then
        result.Should().BeEquivalentTo(calendarEvent);
        calendarEventBrokerMock.Verify(x => x.GetAllCalendarEvents(false), Times.Once);
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}








