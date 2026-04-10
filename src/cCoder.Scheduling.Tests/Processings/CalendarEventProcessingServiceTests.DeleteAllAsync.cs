using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarEventProcessingServiceTests
{
    [Fact]
    public async Task ShouldUseFoundationDeleteAsyncPerItemWhenDeleteAllAsync()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        int id = entity.Id;
        calendarEventServiceMock.Setup(x => x.DeleteAsync(id)).Returns(ValueTask.CompletedTask);

        // When
        await calendarEventProcessingService.DeleteAllAsync(new[] { entity });

        // Then
        calendarEventServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
        calendarEventServiceMock.VerifyNoOtherCalls();
    }

}









