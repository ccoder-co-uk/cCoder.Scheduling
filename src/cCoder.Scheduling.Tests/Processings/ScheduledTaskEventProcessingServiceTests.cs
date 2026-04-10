using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Foundations.Events;
using cCoder.Scheduling.Services.Processings;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskEventProcessingServiceTests
{
    private readonly Mock<IScheduledTaskEventService> scheduledTaskEventServiceMock;
    private readonly ScheduledTaskEventProcessingService service;

    public ScheduledTaskEventProcessingServiceTests()
    {
        scheduledTaskEventServiceMock = new Mock<IScheduledTaskEventService>(MockBehavior.Strict);
        service = new ScheduledTaskEventProcessingService(scheduledTaskEventServiceMock.Object);
    }

    private static ScheduledTask CreateRandomScheduledTask() =>
        Builder<ScheduledTask>.CreateNew().Build();
}












