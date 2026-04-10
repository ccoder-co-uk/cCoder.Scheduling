using cCoder.Data.Models.Planning;
using EventLibrary.Models;


namespace cCoder.Scheduling.Brokers.Events;

public interface ICalendarEntityEventBroker
{
    ValueTask RaiseCalendarAddEventAsync(EventMessage<Calendar> message);
    ValueTask RaiseCalendarUpdateEventAsync(EventMessage<Calendar> message);
    ValueTask RaiseCalendarDeleteEventAsync(EventMessage<Calendar> message);
}







