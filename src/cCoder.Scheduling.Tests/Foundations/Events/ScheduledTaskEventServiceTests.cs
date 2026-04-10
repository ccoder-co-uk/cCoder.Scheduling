using cCoder.Scheduling.Brokers.Events;
using cCoder.Data;
using Moq;
using cCoder.Data.Models.Security;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class ScheduledTaskEventServiceTests
{
    private readonly Mock<IScheduledTaskEventBroker> scheduledTaskEventBrokerMock;
    private readonly Mock<ICoreAuthInfo> authInfoMock;
    private readonly cCoder.Scheduling.Services.Foundations.Events.ScheduledTaskEventService service;
    private const string CurrentUserId = "test-user";

    public ScheduledTaskEventServiceTests()
    {
        scheduledTaskEventBrokerMock = new Mock<IScheduledTaskEventBroker>(MockBehavior.Strict);
        authInfoMock = new Mock<ICoreAuthInfo>(MockBehavior.Strict);
        scheduledTaskEventBrokerMock = new(MockBehavior.Strict);
        authInfoMock = new();
        authInfoMock.SetupGet(x => x.SSOUserId).Returns(CurrentUserId);
        service = new cCoder.Scheduling.Services.Foundations.Events.ScheduledTaskEventService(
            scheduledTaskEventBrokerMock.Object,
            authInfoMock.Object
        );
    }
}









