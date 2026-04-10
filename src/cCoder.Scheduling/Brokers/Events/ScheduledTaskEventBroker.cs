using cCoder.Data.Models.Planning;
using EventLibrary;
using EventLibrary.Models;


namespace cCoder.Scheduling.Brokers.Events;

public class ScheduledTaskEventBroker(IEventHub eventHub) : IScheduledTaskEventBroker
{
    public ValueTask RaiseScheduledTaskAddEventAsync(EventMessage<ScheduledTask> message) =>
        eventHub.RaiseEventAsync("scheduled_task_add", message);

    public ValueTask RaiseScheduledTaskUpdateEventAsync(EventMessage<ScheduledTask> message) =>
        eventHub.RaiseEventAsync("scheduled_task_update", message);

    public ValueTask RaiseScheduledTaskDeleteEventAsync(EventMessage<ScheduledTask> message) =>
        eventHub.RaiseEventAsync("scheduled_task_delete", message);

    public ValueTask RaiseScheduledTaskExecuteEventAsync(EventMessage<ScheduledTask> message) =>
        eventHub.RaiseEventAsync("scheduled_task_execute", message);
}







