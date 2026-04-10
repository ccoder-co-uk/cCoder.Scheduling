using cCoder.Data.Models.Planning;
using EventLibrary.Models;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class CalendarEventEventServiceTests
{
    [Fact]
    public async Task ShouldMapAndCallBrokerWhenRaiseCalendarEventAddEventAsync()
    {
        // Given
        CalendarEvent entity = new();
        EventMessage<CalendarEvent> actualMessage = null;

        calendarEventEventBrokerMock
            .Setup(x => x.RaiseCalendarEventAddEventAsync(It.IsAny<EventMessage<CalendarEvent>>()))
            .Callback<EventMessage<CalendarEvent>>(message => actualMessage = message)
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseCalendarEventAddEventAsync(entity);

        // Then
        actualMessage.Should().NotBeNull();
        actualMessage!.Data.Should().BeEquivalentTo(entity);
        actualMessage.AuthInfo.Should().NotBeNull();
        actualMessage.AuthInfo.SSOUserId.Should().Be(CurrentUserId);
        calendarEventEventBrokerMock.Verify(
            x => x.RaiseCalendarEventAddEventAsync(It.IsAny<EventMessage<CalendarEvent>>()),
            Times.Once
        );
        calendarEventEventBrokerMock.VerifyNoOtherCalls();
    }

}









