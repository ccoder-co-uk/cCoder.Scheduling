using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldGetThenDeleteThenRaiseDeleteEventAsyncWhenDeleteAsync()
    {
        // Given
        int id = 1;
        Calendar entity = CreateRandomCalendar();
        calendarProcessingServiceMock.Setup(x => x.GetAll(true)).Returns(new[] { entity }.AsQueryable());
        calendarProcessingServiceMock.Setup(x => x.DeleteAsync(id)).Returns(ValueTask.CompletedTask);

        calendarEntityEventProcessingServiceMock
            .Setup(x => x.RaiseCalendarDeleteEventAsync(entity))
            .Returns(ValueTask.CompletedTask);

        // When
        await orchestrationService.DeleteAsync(id);

        // Then
        calendarProcessingServiceMock.Verify(x => x.GetAll(true), Times.Once);
        calendarProcessingServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
        calendarEntityEventProcessingServiceMock.Verify(x => x.RaiseCalendarDeleteEventAsync(entity), Times.Once);
    }

}









