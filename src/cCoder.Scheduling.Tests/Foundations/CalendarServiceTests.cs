using cCoder.Scheduling.Brokers.Storage;
using AuthorizationBroker = cCoder.Scheduling.Brokers.AuthorizationBroker;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;
using cCoder.Data.Models.Planning;
using cCoder.Scheduling.Services.Foundations;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class CalendarServiceTests
{
    private readonly Mock<ICalendarBroker> calendarBrokerMock;
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock;
    private readonly CalendarService calendarService;

    public CalendarServiceTests()
    {
        calendarBrokerMock = new Mock<ICalendarBroker>(MockBehavior.Strict);
        authorizationBrokerMock = new Mock<IAuthorizationBroker>(MockBehavior.Strict);
        calendarService = new CalendarService(
            calendarBrokerMock.Object,
            authorizationBrokerMock.Object
        );
    }

    private static Calendar CreateRandomCalendar(int id = 42, int appId = 7)
    {
        Calendar calendar = Builder<Calendar>
            .CreateNew()
            .With(x => x.Id = id)
            .With(x => x.AppId = appId)
            .With(x => x.Name = $"Calendar-{Guid.NewGuid():N}")
            .With(x => x.Description = $"Description-{Guid.NewGuid():N}")
            .With(x => x.Events = Array.Empty<CalendarEvent>())
            .Build();

        return calendar;
    }
}














