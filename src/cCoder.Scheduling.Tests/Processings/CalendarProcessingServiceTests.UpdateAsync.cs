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
    public async Task ShouldDelegateToFoundationServiceWhenUpdateAsync()
    {
        // Given
        Calendar entity = CreateRandomCalendar();
        calendarServiceMock.Setup(x => x.UpdateAsync(entity)).ReturnsAsync(entity);

        // When
        Calendar result = await calendarProcessingService.UpdateAsync(entity);

        // Then
        result.Should().BeSameAs(entity);
        calendarServiceMock.Verify(x => x.UpdateAsync(entity), Times.Once);
        calendarServiceMock.VerifyNoOtherCalls();
    }

}









