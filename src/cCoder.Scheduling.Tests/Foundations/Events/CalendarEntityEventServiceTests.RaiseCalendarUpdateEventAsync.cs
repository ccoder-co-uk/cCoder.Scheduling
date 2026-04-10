using cCoder.Data.Models.Planning;
using EventLibrary.Models;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class CalendarEntityEventServiceTests
{
    [Fact]
    public async Task ShouldMapAndCallBrokerWhenRaiseCalendarUpdateEventAsync()
    {
        // Given
        Calendar entity = new();
        EventMessage<Calendar> actualMessage = null;

        calendarEventBrokerMock
            .Setup(x => x.RaiseCalendarUpdateEventAsync(It.IsAny<EventMessage<Calendar>>()))
            .Callback<EventMessage<Calendar>>(message => actualMessage = message)
            .Returns(ValueTask.CompletedTask);

        // When
        await service.RaiseCalendarUpdateEventAsync(entity);

        // Then
        actualMessage.Should().NotBeNull();
        actualMessage!.Data.Should().BeEquivalentTo(entity);
        actualMessage.AuthInfo.Should().NotBeNull();
        actualMessage.AuthInfo.SSOUserId.Should().Be(CurrentUserId);
        calendarEventBrokerMock.Verify(
            x => x.RaiseCalendarUpdateEventAsync(It.IsAny<EventMessage<Calendar>>()),
            Times.Once
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
    }

}









