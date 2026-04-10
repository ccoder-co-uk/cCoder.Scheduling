using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarEventOrchestrationServiceTests
{
    [Fact]
    public void ShouldReturnProcessingResultsWhenGetAll()
    {
        // Given
        IQueryable<CalendarEvent> entities = new[] { CreateRandomCalendarEvent() }.AsQueryable();
        calendarEventProcessingServiceMock.Setup(x => x.GetAll(true)).Returns(entities);

        // When
        IQueryable<CalendarEvent> result = orchestrationService.GetAll(true);

        // Then
        result.Should().BeSameAs(entities);
        calendarEventProcessingServiceMock.Verify(x => x.GetAll(true), Times.Once);
        calendarEventProcessingServiceMock.VerifyNoOtherCalls();
        calendarEventEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









