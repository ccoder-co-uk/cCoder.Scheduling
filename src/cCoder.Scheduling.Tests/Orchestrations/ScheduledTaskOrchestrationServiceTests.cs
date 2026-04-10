using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Orchestrations;
using cCoder.Scheduling.Services.Processings;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class ScheduledTaskOrchestrationServiceTests
{
    private readonly Mock<IScheduledTaskProcessingService> scheduledTaskProcessingServiceMock;
    private readonly Mock<IScheduledTaskEventProcessingService> scheduledTaskEventProcessingServiceMock;
    private readonly ScheduledTaskOrchestrationService orchestrationService;

    public ScheduledTaskOrchestrationServiceTests()
    {
        scheduledTaskProcessingServiceMock = new Mock<IScheduledTaskProcessingService>(MockBehavior.Strict);
        scheduledTaskEventProcessingServiceMock = new Mock<IScheduledTaskEventProcessingService>(MockBehavior.Strict);
        orchestrationService = new ScheduledTaskOrchestrationService(
            scheduledTaskProcessingServiceMock.Object,
            scheduledTaskEventProcessingServiceMock.Object
        );
    }

    private static ScheduledTask CreateRandomScheduledTask() =>
        Builder<ScheduledTask>.CreateNew().Build();
}










