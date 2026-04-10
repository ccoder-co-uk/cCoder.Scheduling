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
    public async Task ShouldDelegateToFoundationServiceWhenAddAsync()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        calendarEventServiceMock.Setup(x => x.AddAsync(entity)).ReturnsAsync(entity);

        // When
        CalendarEvent result = await calendarEventProcessingService.AddAsync(entity);

        // Then
        result.Should().BeSameAs(entity);
        calendarEventServiceMock.Verify(x => x.AddAsync(entity), Times.Once);
        calendarEventServiceMock.VerifyNoOtherCalls();
    }

}









