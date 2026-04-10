using cCoder.Scheduling.Services.Foundations.Events;


namespace cCoder.Scheduling.Exposures.EventHandlers;

internal class SchedulingEventHandlers(IEventHandlerService eventHandlerService)
    : ISchedulingEventHandlers
{
    public void ListenToAllEvents() => eventHandlerService.ListenToAllEvents();
}


