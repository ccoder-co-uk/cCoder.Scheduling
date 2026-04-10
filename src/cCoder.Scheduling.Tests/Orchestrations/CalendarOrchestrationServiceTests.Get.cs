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
    public void ShouldReturnProcessingResultWhenGet()
    {
        // Given
        int id = 1;
        Calendar entity = CreateRandomCalendar();
        calendarProcessingServiceMock.Setup(x => x.Get(id)).Returns(entity);

        // When
        Calendar result = orchestrationService.Get(id);

        // Then
        result.Should().BeSameAs(entity);
        calendarProcessingServiceMock.Verify(x => x.Get(id), Times.Once);
        calendarProcessingServiceMock.VerifyNoOtherCalls();
        calendarEntityEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









