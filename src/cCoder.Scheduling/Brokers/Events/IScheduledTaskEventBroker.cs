using cCoder.Data.Models.Planning;
using EventLibrary.Models;


namespace cCoder.Scheduling.Brokers.Events;

public interface IScheduledTaskEventBroker
{
    ValueTask RaiseScheduledTaskAddEventAsync(EventMessage<ScheduledTask> message);
    ValueTask RaiseScheduledTaskUpdateEventAsync(EventMessage<ScheduledTask> message);
    ValueTask RaiseScheduledTaskDeleteEventAsync(EventMessage<ScheduledTask> message);
}







