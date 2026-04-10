using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarEventProcessingServiceTests
{
    [Fact]
    public void ShouldDelegateToFoundationServiceWhenGetAll()
    {
        // Given
        IQueryable<CalendarEvent> entities = new[] { CreateRandomCalendarEvent() }.AsQueryable();
        calendarEventServiceMock.Setup(x => x.GetAll()).Returns(entities);

        // When
        IQueryable<CalendarEvent> result = calendarEventProcessingService.GetAll();

        // Then
        result.Should().BeSameAs(entities);
        calendarEventServiceMock.Verify(x => x.GetAll(), Times.Once);
        calendarEventServiceMock.VerifyNoOtherCalls();
    }

}









