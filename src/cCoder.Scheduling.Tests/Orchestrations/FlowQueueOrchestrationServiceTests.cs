using cCoder.Scheduling.Services.Foundations;
using cCoder.Scheduling.Services.Orchestrations;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class FlowQueueOrchestrationServiceTests
{
    private readonly Mock<IFlowQueueService> flowQueueServiceMock;
    private readonly Mock<cCoder.Scheduling.Brokers.IJsonBroker> jsonBrokerMock;
    private readonly FlowQueueOrchestrationService orchestrationService;

    public FlowQueueOrchestrationServiceTests()
    {
        flowQueueServiceMock = new Mock<IFlowQueueService>(MockBehavior.Strict);
        jsonBrokerMock = new Mock<cCoder.Scheduling.Brokers.IJsonBroker>(MockBehavior.Strict);
        orchestrationService = new FlowQueueOrchestrationService(
            flowQueueServiceMock.Object,
            jsonBrokerMock.Object);
    }
}

