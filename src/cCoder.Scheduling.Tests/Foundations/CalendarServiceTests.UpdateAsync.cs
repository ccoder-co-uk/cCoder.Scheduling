using System.Security;
using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarServiceTests
{
    [Fact]
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForUpdateAsync()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        Calendar submitted = null;

        calendarBrokerMock.Setup(x => x.GetAppId(It.IsAny<Calendar>())).Returns((int?)calendar.AppId);

        authorizationBrokerMock.Setup(x => x.Authorize((int?)calendar.AppId, "Calendar_update"));

        calendarBrokerMock
            .Setup(x => x.UpdateCalendarAsync(It.IsAny<Calendar>()))
            .Callback<Calendar>(candidate => submitted = candidate)
            .ReturnsAsync((Calendar value) => value);

        // When
        Calendar result = await calendarService.UpdateAsync(calendar);

        // Then
        result.Should().NotBeSameAs(calendar);
        submitted.Should().NotBeNull();
        submitted.Should().BeEquivalentTo(calendar, options => options.Excluding(candidate => candidate.Events));
        result.Should().BeEquivalentTo(calendar, options => options.Excluding(candidate => candidate.Events));
        calendarBrokerMock.Verify(x => x.UpdateCalendarAsync(It.IsAny<Calendar>()), Times.Once);
        calendarBrokerMock.Verify(x => x.GetAppId(It.IsAny<Calendar>()), Times.AtMostOnce());
        calendarBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)calendar.AppId, "Calendar_update"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksUpdatePrivilegeForUpdateAsync()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)calendar.AppId, "Calendar_update"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await calendarService.UpdateAsync(calendar);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        calendarBrokerMock.Verify(x => x.GetAppId(It.IsAny<Calendar>()), Times.AtMostOnce());
        calendarBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)calendar.AppId, "Calendar_update"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}










