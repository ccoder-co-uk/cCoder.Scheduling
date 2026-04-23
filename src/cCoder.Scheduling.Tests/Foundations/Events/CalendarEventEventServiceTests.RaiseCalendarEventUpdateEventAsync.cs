using cCoder.Data.Models.Planning;
using cCoder.Eventing.Models;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class CalendarEventEventServiceTests
{
    [Fact]
    public async Task ShouldMapAndCallBrokerWhenRaiseCalendarEventUpdateEventAsync()
    {
        // Given
        CalendarEvent entity = new();
        EventMessage<CalendarEvent> actualMessage = null;

        calendarEventEventBrokerMock
            .Setup(x =>
                x.RaiseCalendarEventUpdateEventAsync(It.IsAny<EventMessage<CalendarEvent>>())
            )
            .Callback<EventMessage<CalendarEvent>>(message => actualMessage = message)
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseCalendarEventUpdateEventAsync(entity);

        // Then
        actualMessage.Should().NotBeNull();
        actualMessage!.Data.Should().BeEquivalentTo(entity);
        actualMessage.AuthInfo.Should().NotBeNull();
        actualMessage.AuthInfo.SSOUserId.Should().Be(CurrentUserId);
        calendarEventEventBrokerMock.Verify(
            x => x.RaiseCalendarEventUpdateEventAsync(It.IsAny<EventMessage<CalendarEvent>>()),
            Times.Once
        );
        calendarEventEventBrokerMock.VerifyNoOtherCalls();
    }

}









