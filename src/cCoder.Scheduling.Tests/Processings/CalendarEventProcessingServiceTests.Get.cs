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
    public void ShouldDelegateToFoundationServiceWhenGet()
    {
        // Given
        CalendarEvent entity = CreateRandomCalendarEvent();
        var id = entity.Id;
        calendarEventServiceMock.Setup(x => x.Get(id)).Returns(entity);

        // When
        CalendarEvent result = calendarEventProcessingService.Get(id);

        // Then
        result.Should().BeSameAs(entity);
        calendarEventServiceMock.Verify(x => x.Get(id), Times.Once);
        calendarEventServiceMock.VerifyNoOtherCalls();
    }

}









