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
    public async Task ShouldCallProcessingThenRaiseAddEventAsyncWhenAddAsync()
    {
        // Given
        Calendar entity = CreateRandomCalendar();
        calendarProcessingServiceMock.Setup(x => x.AddAsync(entity)).ReturnsAsync(entity);

        calendarEntityEventProcessingServiceMock
            .Setup(x => x.RaiseCalendarAddEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        Calendar result = await orchestrationService.AddAsync(entity);

        // Then
        result.Should().BeSameAs(entity);
        calendarProcessingServiceMock.Verify(x => x.AddAsync(entity), Times.Once);
        calendarEntityEventProcessingServiceMock.Verify(x => x.RaiseCalendarAddEventAsync(entity), Times.Once);
    }

}









