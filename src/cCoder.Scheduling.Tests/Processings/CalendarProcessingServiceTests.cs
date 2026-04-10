using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Foundations;
using cCoder.Scheduling.Services.Processings;
using Moq;
using DataUser = cCoder.Data.Models.Security.User;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarProcessingServiceTests
{
    private readonly Mock<ICalendarEventProcessingService> calendarEventServiceMock = new();
    private DataUser currentUser = TestUsers.WithoutPrivileges();
    private readonly Mock<ICalendarService> calendarServiceMock = new();
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock = new();
    private readonly CalendarProcessingService calendarProcessingService;

    public CalendarProcessingServiceTests()
    {
        calendarProcessingService = new CalendarProcessingService(
            calendarServiceMock.Object,
            calendarEventServiceMock.Object,
            authorizationBrokerMock.Object
        );
    }

    private static Calendar CreateRandomCalendar() =>
        new()
        {
            Id = Random.Shared.Next(1, 10000),
            AppId = 1,
            Name = $"Calendar-{Guid.NewGuid():N}",
            Description = $"Description-{Guid.NewGuid():N}",
            App = null!,
            Events = [],
        };
}














