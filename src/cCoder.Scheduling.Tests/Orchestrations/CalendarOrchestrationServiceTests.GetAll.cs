using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarOrchestrationServiceTests
{
    [Fact]
    public void ShouldReturnProcessingResultsWhenGetAll()
    {
        // Given
        IQueryable<Calendar> entities = new[] { CreateRandomCalendar() }.AsQueryable();
        calendarProcessingServiceMock.Setup(x => x.GetAll(true)).Returns(entities);

        // When
        IQueryable<Calendar> result = orchestrationService.GetAll(true);

        // Then
        result.Should().BeSameAs(entities);
        calendarProcessingServiceMock.Verify(x => x.GetAll(true), Times.Once);
        calendarProcessingServiceMock.VerifyNoOtherCalls();
        calendarEntityEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









