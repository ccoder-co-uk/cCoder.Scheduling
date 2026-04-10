using cCoder.Scheduling.Brokers.Events;
using cCoder.Data;
using Moq;
using cCoder.Data.Models.Security;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class CalendarEntityEventServiceTests
{
    private readonly Mock<ICalendarEntityEventBroker> calendarEventBrokerMock;
    private readonly Mock<ICoreAuthInfo> authInfoMock;
    private readonly cCoder.Scheduling.Services.Foundations.Events.CalendarEntityEventService service;
    private const string CurrentUserId = "test-user";

    public CalendarEntityEventServiceTests()
    {
        calendarEventBrokerMock = new Mock<ICalendarEntityEventBroker>(MockBehavior.Strict);
        authInfoMock = new Mock<ICoreAuthInfo>(MockBehavior.Strict);
        calendarEventBrokerMock = new(MockBehavior.Strict);
        authInfoMock = new();
        authInfoMock.SetupGet(x => x.SSOUserId).Returns(CurrentUserId);
        service = new cCoder.Scheduling.Services.Foundations.Events.CalendarEntityEventService(
            calendarEventBrokerMock.Object,
            authInfoMock.Object
        );
    }
}









