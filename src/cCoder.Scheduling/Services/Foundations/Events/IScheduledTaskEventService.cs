using cCoder.Data.Models.Planning;


namespace cCoder.Scheduling.Services.Foundations.Events;

public interface IScheduledTaskEventService
{
    ValueTask RaiseScheduledTaskAddEventAsync(ScheduledTask entity);
    ValueTask RaiseScheduledTaskUpdateEventAsync(ScheduledTask entity);
    ValueTask RaiseScheduledTaskDeleteEventAsync(ScheduledTask entity);
    ValueTask RaiseScheduledTaskExecuteEventAsync(ScheduledTask entity);
}










