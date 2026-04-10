using cCoder.Data.Models.Planning;
using EventLibrary.Models;


namespace cCoder.Scheduling.Brokers.Events;

public interface ICalendarEventEventBroker
{
    ValueTask RaiseCalendarEventAddEventAsync(EventMessage<CalendarEvent> message);
    ValueTask RaiseCalendarEventUpdateEventAsync(EventMessage<CalendarEvent> message);
    ValueTask RaiseCalendarEventDeleteEventAsync(EventMessage<CalendarEvent> message);
}







