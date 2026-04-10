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

public partial class CalendarEntityEventProcessingServiceTests
{
    private readonly Mock<ICalendarEntityEventService> calendarEntityEventServiceMock;
    private readonly CalendarEntityEventProcessingService service;

    public CalendarEntityEventProcessingServiceTests()
    {
        calendarEntityEventServiceMock = new Mock<ICalendarEntityEventService>(MockBehavior.Strict);
        service = new CalendarEntityEventProcessingService(calendarEntityEventServiceMock.Object);
    }

    private static Calendar CreateRandomCalendar() =>
        Builder<Calendar>.CreateNew().Build();
}












