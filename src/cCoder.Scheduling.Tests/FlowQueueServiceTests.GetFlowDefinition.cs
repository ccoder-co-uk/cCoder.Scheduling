using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Scheduling.Tests;

public partial class FlowQueueServiceTests
{
    [Fact]
    public void ShouldMapFlowDefinitionWhenGetFlowDefinition()
    {
        Guid id = Guid.NewGuid();
        var dataFlowDefinition = CreateDataFlowDefinition(id);

        brokerMock.Setup(x => x.GetFlowDefinition(id)).Returns(dataFlowDefinition);

        var result = service.GetFlowDefinition(id);

        result.Id.Should().Be(dataFlowDefinition.Id);
        result.AppId.Should().Be(dataFlowDefinition.AppId);
        result.Name.Should().Be(dataFlowDefinition.Name);
        result.Description.Should().Be(dataFlowDefinition.Description);
        result.DefinitionJson.Should().Be(dataFlowDefinition.DefinitionJson);
        brokerMock.Verify(x => x.GetFlowDefinition(id), Times.Once);
        brokerMock.VerifyNoOtherCalls();
    }
}

