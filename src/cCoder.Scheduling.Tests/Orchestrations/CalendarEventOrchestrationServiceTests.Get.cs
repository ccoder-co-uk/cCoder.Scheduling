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
    public void ShouldReturnProcessingResultWhenGet()
    {
        // Given
        int id = 1;
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventProcessingServiceMock.Setup(x => x.Get(id)).Returns(entity);

        // When
        CalendarEvent result = orchestrationService.Get(id);

        // Then
        result.Should().BeSameAs(entity);
        calendarEventProcessingServiceMock.Verify(x => x.Get(id), Times.Once);
        calendarEventProcessingServiceMock.VerifyNoOtherCalls();
        calendarEventEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









