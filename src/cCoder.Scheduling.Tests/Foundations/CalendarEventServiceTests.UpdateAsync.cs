using System.Security;
using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarEventServiceTests
{
    [Fact]
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForUpdateAsync()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        CalendarEvent submitted = null;

        calendarEventBrokerMock.Setup(x => x.GetAppId(It.IsAny<CalendarEvent>())).Returns((int?)7);
        authorizationBrokerMock.Setup(x => x.Authorize((int?)7, "CalendarEvent_update"));

        calendarEventBrokerMock
            .Setup(x => x.UpdateCalendarEventAsync(It.IsAny<CalendarEvent>()))
            .Callback<CalendarEvent>(candidate => submitted = candidate)
            .ReturnsAsync((CalendarEvent value) => value);

        // When
        CalendarEvent result = await calendarEventService.UpdateAsync(calendarEvent);

        // Then
        result.Should().BeSameAs(calendarEvent);
        submitted.Should().NotBeNull();
        submitted.Should().NotBeSameAs(calendarEvent);
        result.Should().NotBeSameAs(submitted);
        submitted.Should().BeEquivalentTo(calendarEvent);
        result.Should().BeEquivalentTo(calendarEvent);
        calendarEventBrokerMock.Verify(
            x => x.UpdateCalendarEventAsync(It.IsAny<CalendarEvent>()),
            Times.Once
        );
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "CalendarEvent_update"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksUpdatePrivilegeForUpdateAsync()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        calendarEventBrokerMock.Setup(x => x.GetAppId(It.IsAny<CalendarEvent>())).Returns((int?)7);
        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)7, "CalendarEvent_update"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await calendarEventService.UpdateAsync(calendarEvent);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "CalendarEvent_update"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}










