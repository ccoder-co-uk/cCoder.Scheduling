using cCoder.Scheduling.Brokers.Events;
using cCoder.Data;
using Moq;
using cCoder.Data.Models.Security;


namespace cCoder.Core.Services.Tests.Planning.Foundations.Events;

public partial class CalendarEventEventServiceTests
{
    private readonly Mock<ICalendarEventEventBroker> calendarEventEventBrokerMock;
    private readonly Mock<ICoreAuthInfo> authInfoMock;
    private readonly cCoder.Scheduling.Services.Foundations.Events.CalendarEventEventService service;
    private const string CurrentUserId = "test-user";

    public CalendarEventEventServiceTests()
    {
        calendarEventEventBrokerMock = new Mock<ICalendarEventEventBroker>(MockBehavior.Strict);
        authInfoMock = new Mock<ICoreAuthInfo>(MockBehavior.Strict);
        calendarEventEventBrokerMock = new(MockBehavior.Strict);
        authInfoMock = new();
        authInfoMock.SetupGet(x => x.SSOUserId).Returns(CurrentUserId);
        service = new cCoder.Scheduling.Services.Foundations.Events.CalendarEventEventService(
            calendarEventEventBrokerMock.Object,
            authInfoMock.Object
        );
    }
}









