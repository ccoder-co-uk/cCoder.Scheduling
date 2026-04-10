using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;

namespace cCoder.Scheduling.Services.Orchestrations;

internal class AppOrchestrationService(
    IScheduledTaskOrchestrationService scheduledTaskOrchestrationService,
    ICalendarOrchestrationService calendarOrchestrationService
) : IAppOrchestrationService
{
    public async ValueTask AddAsync(App app)
    {
        StampTasks(app);
        StampCalendars(app);
        _ = await scheduledTaskOrchestrationService.AddOrUpdate(app.Tasks ?? []);
        _ = await calendarOrchestrationService.AddOrUpdate(app.Calendars ?? []);
    }

    public async ValueTask UpdateAsync(App app)
    {
        StampTasks(app);
        StampCalendars(app);
        _ = await scheduledTaskOrchestrationService.AddOrUpdate(app.Tasks ?? []);
        _ = await calendarOrchestrationService.AddOrUpdate(app.Calendars ?? []);
    }

    public async ValueTask DeleteAsync(int appId)
    {
        await scheduledTaskOrchestrationService.DeleteByAppIdAsync(appId);
        await calendarOrchestrationService.DeleteByAppIdAsync(appId);
    }

    private static void StampTasks(App app)
    {
        foreach (ScheduledTask task in app.Tasks ?? [])
            task.AppId = app.Id;
    }

    private static void StampCalendars(App app)
    {
        foreach (Calendar calendar in app.Calendars ?? [])
            calendar.AppId = app.Id;
    }
}

