using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Foundations.Events;


namespace cCoder.Scheduling.Services.Processings;

internal class ScheduledTaskEventProcessingService(IScheduledTaskEventService eventService) : IScheduledTaskEventProcessingService
{
    public ValueTask RaiseScheduledTaskAddEventAsync(ScheduledTask entity) => eventService.RaiseScheduledTaskAddEventAsync(entity);

    public ValueTask RaiseScheduledTaskUpdateEventAsync(ScheduledTask entity) => eventService.RaiseScheduledTaskUpdateEventAsync(entity);

    public ValueTask RaiseScheduledTaskDeleteEventAsync(ScheduledTask entity) => eventService.RaiseScheduledTaskDeleteEventAsync(entity);

    public ValueTask RaiseScheduledTaskExecuteEventAsync(ScheduledTask entity) => eventService.RaiseScheduledTaskExecuteEventAsync(entity);
}









