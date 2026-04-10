using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Scheduling.Tests;

public partial class FlowQueueServiceTests
{
    [Fact]
    public async Task ShouldMapAndPersistFlowInstanceWhenAddAsync()
    {
        FlowInstanceData entity = new()
        {
            Id = Guid.NewGuid(),
            FlowDefinitionId = Guid.NewGuid(),
            State = "Queued",
            Caller = "user-1",
            Start = DateTimeOffset.UtcNow,
            ContextString = "{\"ExecutionState\":\"Queued\"}",
        };

        brokerMock
            .Setup(x => x.AddFlowInstanceDataAsync(It.IsAny<cCoder.Data.Models.Workflow.FlowInstanceData>()))
            .ReturnsAsync((cCoder.Data.Models.Workflow.FlowInstanceData data) => data);

        FlowInstanceData result = await service.AddAsync(entity);

        result.Id.Should().Be(entity.Id);
        result.FlowDefinitionId.Should().Be(entity.FlowDefinitionId);
        result.State.Should().Be(entity.State);
        result.Caller.Should().Be(entity.Caller);
        result.ContextString.Should().Be(entity.ContextString);
        brokerMock.Verify(
            x => x.AddFlowInstanceDataAsync(
                It.Is<cCoder.Data.Models.Workflow.FlowInstanceData>(data =>
                    data.Id == entity.Id
                    && data.FlowDefinitionId == entity.FlowDefinitionId
                    && data.State == entity.State
                    && data.Caller == entity.Caller
                    && data.ContextString == entity.ContextString)),
            Times.Once);
        brokerMock.VerifyNoOtherCalls();
    }
}


