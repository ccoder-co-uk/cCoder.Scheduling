using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;


namespace cCoder.Scheduling.Services.Processings;

public interface ICalendarEventEventProcessingService
{
    ValueTask RaiseCalendarEventAddEventAsync(CalendarEvent entity);
    ValueTask RaiseCalendarEventUpdateEventAsync(CalendarEvent entity);
    ValueTask RaiseCalendarEventDeleteEventAsync(CalendarEvent entity);
}









