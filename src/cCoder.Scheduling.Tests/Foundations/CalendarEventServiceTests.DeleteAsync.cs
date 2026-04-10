using System.Security;
using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarEventServiceTests
{
    [Fact]
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForDeleteAsync()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        calendarEventBrokerMock
            .Setup(x => x.GetAllCalendarEvents(true))
            .Returns(new[] { calendarEvent }.AsQueryable());

        calendarEventBrokerMock.Setup(x => x.GetAppId(It.IsAny<CalendarEvent>())).Returns((int?)7);

        authorizationBrokerMock.Setup(x => x.Authorize((int?)7, "CalendarEvent_delete"));

        calendarEventBrokerMock
            .Setup(
                x =>
                    x.DeleteCalendarEventAsync(
                        It.Is<CalendarEvent>(candidate => candidate.Id == calendarEvent.Id)
                    )
            )
            .ReturnsAsync(1);

        // When
        await calendarEventService.DeleteAsync(calendarEvent.Id);

        // Then
        calendarEventBrokerMock.Verify(x => x.GetAllCalendarEvents(true), Times.Once);
        calendarEventBrokerMock.Verify(
            x =>
                x.DeleteCalendarEventAsync(
                    It.Is<CalendarEvent>(candidate => candidate.Id == calendarEvent.Id)
                ),
            Times.Once
        );
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "CalendarEvent_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksDeletePrivilegeForDeleteAsync()
    {
        // Given
        CalendarEvent calendarEvent = CreateRandomCalendarEvent();

        calendarEventBrokerMock
            .Setup(x => x.GetAllCalendarEvents(true))
            .Returns(new[] { calendarEvent }.AsQueryable());

        calendarEventBrokerMock.Setup(x => x.GetAppId(It.IsAny<CalendarEvent>())).Returns((int?)7);
        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)7, "CalendarEvent_delete"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await calendarEventService.DeleteAsync(calendarEvent.Id);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        calendarEventBrokerMock.Verify(x => x.GetAllCalendarEvents(true), Times.Once);
        calendarEventBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<CalendarEvent>()),
            Times.AtMostOnce()
        );
        calendarEventBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "CalendarEvent_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}









