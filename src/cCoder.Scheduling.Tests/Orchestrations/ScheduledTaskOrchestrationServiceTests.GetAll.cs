using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class ScheduledTaskOrchestrationServiceTests
{
    [Fact]
    public void ShouldReturnProcessingResultsWhenGetAll()
    {
        // Given
        IQueryable<ScheduledTask> entities = new[] { CreateRandomScheduledTask() }.AsQueryable();
        scheduledTaskProcessingServiceMock.Setup(x => x.GetAll(true)).Returns(entities);

        // When
        IQueryable<ScheduledTask> result = orchestrationService.GetAll(true);

        // Then
        result.Should().BeSameAs(entities);
        scheduledTaskProcessingServiceMock.Verify(x => x.GetAll(true), Times.Once);
        scheduledTaskProcessingServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









