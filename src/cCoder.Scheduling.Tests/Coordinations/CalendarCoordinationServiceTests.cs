using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Coordinations;
using cCoder.Scheduling.Services.Orchestrations;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Coordinations;

public partial class CalendarCoordinationServiceTests
{
    private readonly Mock<ICalendarEventOrchestrationService> calendarEventOrchestrationServiceMock;
    private readonly CalendarCoordinationService coordinationService;

    public CalendarCoordinationServiceTests()
    {
        calendarEventOrchestrationServiceMock = new Mock<ICalendarEventOrchestrationService>(
            MockBehavior.Strict
        );

        coordinationService = new CalendarCoordinationService(
            calendarEventOrchestrationServiceMock.Object
        );
    }

    private static Calendar CreateRandomCalendar() =>
        Builder<Calendar>
            .CreateNew()
            .With(calendar => calendar.Events = [Builder<CalendarEvent>.CreateNew().Build()])
            .Build();
}








