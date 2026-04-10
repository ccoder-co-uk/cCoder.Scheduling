using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskProcessingServiceTests
{
    [Fact]
    public void ShouldDelegateToFoundationServiceWhenGetAll()
    {
        // Given
        IQueryable<ScheduledTask> entities = new[] { CreateRandomScheduledTask() }.AsQueryable();
        scheduledTaskServiceMock.Setup(x => x.GetAll()).Returns(entities);

        // When
        IQueryable<ScheduledTask> result = scheduledTaskProcessingService.GetAll();

        // Then
        result.Should().BeSameAs(entities);
        scheduledTaskServiceMock.Verify(x => x.GetAll(), Times.Once);
        scheduledTaskServiceMock.VerifyNoOtherCalls();
    }

}









