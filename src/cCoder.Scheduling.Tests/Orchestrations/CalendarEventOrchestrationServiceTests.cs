using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Orchestrations;
using cCoder.Scheduling.Services.Processings;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class CalendarEventOrchestrationServiceTests
{
    private readonly Mock<ICalendarEventProcessingService> calendarEventProcessingServiceMock;
    private readonly Mock<ICalendarEventEventProcessingService> calendarEventEventProcessingServiceMock;
    private readonly CalendarEventOrchestrationService orchestrationService;

    public CalendarEventOrchestrationServiceTests()
    {
        calendarEventProcessingServiceMock = new Mock<ICalendarEventProcessingService>(MockBehavior.Strict);
        calendarEventEventProcessingServiceMock = new Mock<ICalendarEventEventProcessingService>(MockBehavior.Strict);
        orchestrationService = new CalendarEventOrchestrationService(
            calendarEventProcessingServiceMock.Object,
            calendarEventEventProcessingServiceMock.Object
        );
    }

    private static CalendarEvent CreateRandomCalendarEvent() =>
        Builder<CalendarEvent>.CreateNew().Build();
}










