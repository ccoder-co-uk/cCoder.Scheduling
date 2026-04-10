using cCoder.Scheduling.Brokers.Storage;
using AuthorizationBroker = cCoder.Scheduling.Brokers.AuthorizationBroker;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;
using cCoder.Data.Models.Planning;
using cCoder.Scheduling.Services.Foundations;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarEventServiceTests
{
    private readonly Mock<ICalendarEventBroker> calendarEventBrokerMock;
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock;
    private readonly CalendarEventService calendarEventService;

    public CalendarEventServiceTests()
    {
        calendarEventBrokerMock = new Mock<ICalendarEventBroker>(MockBehavior.Strict);
        authorizationBrokerMock = new Mock<IAuthorizationBroker>(MockBehavior.Strict);
        calendarEventService = new CalendarEventService(
            calendarEventBrokerMock.Object,
            authorizationBrokerMock.Object
        );
    }

    private static CalendarEvent CreateRandomCalendarEvent(int id = 42, int calendarId = 9)
    {
        CalendarEvent calendarEvent = Builder<CalendarEvent>
            .CreateNew()
            .With(x => x.Id = id)
            .With(x => x.CalendarId = calendarId)
            .With(x => x.Name = $"Event-{Guid.NewGuid():N}")
            .With(x => x.Description = $"Description-{Guid.NewGuid():N}")
            .With(x => x.Start = DateTimeOffset.UtcNow)
            .With(x => x.DurationInTicks = TimeSpan.FromHours(1).Ticks)
            .Build();

        return calendarEvent;
    }
}














