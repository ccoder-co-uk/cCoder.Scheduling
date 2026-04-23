using System.Security;
using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarEventServiceTests
{
    [Fact]
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForAddAsync()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        CalendarEvent submitted = null;

        calendarEventBrokerMock.Setup(x => x.GetAppId(It.IsAny<CalendarEvent>())).Returns((int?)7);
        authorizationBrokerMock.Setup(x => x.Authorize((int?)7, "CalendarEvent_create"));

        calendarEventBrokerMock
            .Setup(x =>
                x.AddCalendarEventAsync(
                    It.Is<CalendarEvent>(candidate => !ReferenceEquals(candidate, calendarEvent))
                )
            )
            .Callback<CalendarEvent>(candidate => submitted = candidate)
            .ReturnsAsync((CalendarEvent value) => value);

        // When
        CalendarEvent result = await calendarEventService.AddAsync(calendarEvent);

        // Then
        result.Should().BeSameAs(calendarEvent);
        submitted.Should().NotBeNull();
        submitted.Should().NotBeSameAs(calendarEvent);
        result.Should().NotBeSameAs(submitted);

        submitted
            .Should()
            .BeEquivalentTo(calendarEvent, options => options.Excluding(candidate => candidate.Id));

        result
            .Should()
            .BeEquivalentTo(calendarEvent, options => options.Excluding(candidate => candidate.Id));

        calendarEventBrokerMock.Verify(
            x =>
                x.AddCalendarEventAsync(
                    It.Is<CalendarEvent>(candidate => !ReferenceEquals(candidate, calendarEvent))
                ),
            Times.Once
        );
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "CalendarEvent_create"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksCreatePrivilegeForAddAsync()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        calendarEventBrokerMock.Setup(x => x.GetAppId(It.IsAny<CalendarEvent>())).Returns((int?)7);
        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)7, "CalendarEvent_create"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await calendarEventService.AddAsync(calendarEvent);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "CalendarEvent_create"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}










