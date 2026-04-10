using cCoder.Data.Models.Planning;
using EventLibrary;
using EventLibrary.Models;


namespace cCoder.Scheduling.Brokers.Events;

public class CalendarEntityEventBroker(IEventHub eventHub) : ICalendarEntityEventBroker
{
    public ValueTask RaiseCalendarAddEventAsync(EventMessage<Calendar> message) =>
        eventHub.RaiseEventAsync("calendar_add", message);

    public ValueTask RaiseCalendarUpdateEventAsync(EventMessage<Calendar> message) =>
        eventHub.RaiseEventAsync("calendar_update", message);

    public ValueTask RaiseCalendarDeleteEventAsync(EventMessage<Calendar> message) =>
        eventHub.RaiseEventAsync("calendar_delete", message);
}







