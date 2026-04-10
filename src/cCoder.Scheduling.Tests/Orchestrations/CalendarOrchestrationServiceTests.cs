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

public partial class CalendarOrchestrationServiceTests
{
    private readonly Mock<ICalendarProcessingService> calendarProcessingServiceMock;
    private readonly Mock<ICalendarEntityEventProcessingService> calendarEntityEventProcessingServiceMock;
    private readonly CalendarOrchestrationService orchestrationService;

    public CalendarOrchestrationServiceTests()
    {
        calendarProcessingServiceMock = new Mock<ICalendarProcessingService>(MockBehavior.Strict);
        calendarEntityEventProcessingServiceMock = new Mock<ICalendarEntityEventProcessingService>(MockBehavior.Strict);
        orchestrationService = new CalendarOrchestrationService(
            calendarProcessingServiceMock.Object,
            calendarEntityEventProcessingServiceMock.Object
        );
    }

    private static Calendar CreateRandomCalendar() => Builder<Calendar>.CreateNew().Build();
}










