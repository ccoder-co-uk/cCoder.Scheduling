using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Foundations;
using cCoder.Scheduling.Services.Processings;
using FizzWare.NBuilder;
using Moq;
using DataUser = cCoder.Data.Models.Security.User;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class CalendarEventProcessingServiceTests
{
    private DataUser currentUser = TestUsers.WithoutPrivileges();
    private readonly Mock<ICalendarEventService> calendarEventServiceMock = new();
    private readonly CalendarEventProcessingService calendarEventProcessingService;

    public CalendarEventProcessingServiceTests()
    {
        calendarEventProcessingService = new CalendarEventProcessingService(calendarEventServiceMock.Object);
    }

    private static CalendarEvent CreateRandomCalendarEvent() =>
        Builder<CalendarEvent>.CreateNew().Build();
}











