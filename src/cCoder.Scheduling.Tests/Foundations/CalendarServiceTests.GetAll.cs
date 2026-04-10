using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarServiceTests
{
    [Fact]
    public void ShouldDelegateToBrokerWhenGetAll()
    {
        // Given
        cCoder.Data.Models.Planning.Calendar calendar = CreateRandomCalendar();
        IQueryable<cCoder.Data.Models.Planning.Calendar> calendars = new[] { calendar }.AsQueryable();

        calendarBrokerMock.Setup(x => x.GetAllCalendars(false)).Returns(calendars);

        // When
        IQueryable<Calendar> result = calendarService.GetAll();

        // Then
        result.Should().BeEquivalentTo(calendars.Select(item => (Calendar)item));
        calendarBrokerMock.Verify(x => x.GetAllCalendars(false), Times.Once);
        calendarBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<cCoder.Data.Models.Planning.Calendar>()),
            Times.AtMostOnce()
        );
        calendarBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}









