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

public partial class CalendarEventEventProcessingServiceTests
{
    private readonly Mock<ICalendarEventEventService> calendarEventEventServiceMock;
    private readonly CalendarEventEventProcessingService service;

    public CalendarEventEventProcessingServiceTests()
    {
        calendarEventEventServiceMock = new Mock<ICalendarEventEventService>(MockBehavior.Strict);
        service = new CalendarEventEventProcessingService(calendarEventEventServiceMock.Object);
    }

    private static CalendarEvent CreateRandomCalendarEvent() =>
        Builder<CalendarEvent>.CreateNew().Build();
}












