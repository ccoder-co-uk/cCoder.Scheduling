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
    public void ShouldDelegateToFoundationServiceWhenGetAll()
    {
        // Given
        IQueryable<Calendar> entities = new[] { CreateRandomCalendar() }.AsQueryable();
        calendarServiceMock.Setup(x => x.GetAll()).Returns(entities);

        // When
        IQueryable<Calendar> result = calendarProcessingService.GetAll();

        // Then
        result.Should().BeSameAs(entities);
        calendarServiceMock.Verify(x => x.GetAll(), Times.Once);
        calendarServiceMock.VerifyNoOtherCalls();
    }

}









