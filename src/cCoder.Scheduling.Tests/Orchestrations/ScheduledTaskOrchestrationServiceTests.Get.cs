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
    public void ShouldReturnProcessingResultWhenGet()
    {
        // Given
        int id = 1;
        ScheduledTask entity = CreateRandomScheduledTask();
        scheduledTaskProcessingServiceMock.Setup(x => x.Get(id)).Returns(entity);

        // When
        ScheduledTask result = orchestrationService.Get(id);

        // Then
        result.Should().BeSameAs(entity);
        scheduledTaskProcessingServiceMock.Verify(x => x.Get(id), Times.Once);
        scheduledTaskProcessingServiceMock.VerifyNoOtherCalls();
        scheduledTaskEventProcessingServiceMock.VerifyNoOtherCalls();
    }

}









