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
    public void ShouldDelegateToFoundationServiceWhenGet()
    {
        // Given
        ScheduledTask entity = CreateRandomScheduledTask();
        var id = entity.Id;
        scheduledTaskServiceMock.Setup(x => x.Get(id)).Returns(entity);

        // When
        ScheduledTask result = scheduledTaskProcessingService.Get(id);

        // Then
        result.Should().BeSameAs(entity);
        scheduledTaskServiceMock.Verify(x => x.Get(id), Times.Once);
        scheduledTaskServiceMock.VerifyNoOtherCalls();
    }

}









