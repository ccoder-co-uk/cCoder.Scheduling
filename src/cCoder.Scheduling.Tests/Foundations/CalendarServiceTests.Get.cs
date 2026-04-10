using cCoder.Data.Models.Planning;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarServiceTests
{
    [Fact]
    public void ShouldDelegateToBrokerWhenGet()
    {
        // Given
        Calendar calendar = CreateRandomCalendar();

        calendarBrokerMock.Setup(x => x.GetAllCalendars(false)).Returns(new[] { calendar }.AsQueryable());

        // When
        Calendar result = calendarService.Get(calendar.Id);

        // Then
        result.Should().BeEquivalentTo(calendar);
        calendarBrokerMock.Verify(x => x.GetAllCalendars(false), Times.Once);
        calendarBrokerMock.Verify(x => x.GetAppId(It.IsAny<Calendar>()), Times.AtMostOnce());
        calendarBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}








