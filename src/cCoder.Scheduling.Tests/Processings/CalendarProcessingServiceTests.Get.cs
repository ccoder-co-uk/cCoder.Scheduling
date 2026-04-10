using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarProcessingServiceTests
{
    [Fact]
    public void ShouldDelegateToFoundationServiceWhenGet()
    {
        // Given
        Calendar entity = CreateRandomCalendar();
        var id = entity.Id;
        calendarServiceMock.Setup(x => x.Get(id)).Returns(entity);

        // When
        Calendar result = calendarProcessingService.Get(id);

        // Then
        result.Should().BeSameAs(entity);
        calendarServiceMock.Verify(x => x.Get(id), Times.Once);
        calendarServiceMock.VerifyNoOtherCalls();
    }

}









