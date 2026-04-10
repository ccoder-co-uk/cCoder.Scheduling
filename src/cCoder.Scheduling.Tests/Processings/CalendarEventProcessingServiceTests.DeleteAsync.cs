using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarEventProcessingServiceTests
{
    [Fact]
    public async Task ShouldDelegateToFoundationServiceWhenDeleteAsync()
    {
        // Given
        int id = 1;
        calendarEventServiceMock.Setup(x => x.DeleteAsync(id)).Returns(ValueTask.CompletedTask);

        // When
        await calendarEventProcessingService.DeleteAsync(id);

        // Then
        calendarEventServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
        calendarEventServiceMock.VerifyNoOtherCalls();
    }

}





