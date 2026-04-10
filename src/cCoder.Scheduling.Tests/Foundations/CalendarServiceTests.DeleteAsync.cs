using System.Security;
using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarServiceTests
{
    [Fact]
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForDeleteAsync()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        calendarBrokerMock.Setup(x => x.GetAllCalendars(true)).Returns(new[] { calendar }.AsQueryable());

        calendarBrokerMock.Setup(x => x.GetAppId(It.IsAny<Calendar>())).Returns((int?)calendar.AppId);
        authorizationBrokerMock.Setup(x => x.Authorize((int?)calendar.AppId, "Calendar_delete"));
        calendarBrokerMock
            .Setup(x => x.DeleteCalendarAsync(It.Is<Calendar>(candidate => candidate.Id == calendar.Id)))
            .ReturnsAsync(1);

        // When
        await calendarService.DeleteAsync(calendar.Id);

        // Then
        calendarBrokerMock.Verify(x => x.GetAllCalendars(true), Times.Once);
        calendarBrokerMock.Verify(
            x => x.DeleteCalendarAsync(It.Is<Calendar>(candidate => candidate.Id == calendar.Id)),
            Times.Once
        );
        calendarBrokerMock.Verify(x => x.GetAppId(It.IsAny<Calendar>()), Times.AtMostOnce());
        calendarBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)calendar.AppId, "Calendar_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksDeletePrivilegeForDeleteAsync()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        calendarBrokerMock.Setup(x => x.GetAllCalendars(true)).Returns(new[] { calendar }.AsQueryable());

        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)calendar.AppId, "Calendar_delete"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await calendarService.DeleteAsync(calendar.Id);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        calendarBrokerMock.Verify(x => x.GetAllCalendars(true), Times.Once);
        calendarBrokerMock.Verify(x => x.GetAppId(It.IsAny<Calendar>()), Times.AtMostOnce());
        calendarBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)calendar.AppId, "Calendar_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}









