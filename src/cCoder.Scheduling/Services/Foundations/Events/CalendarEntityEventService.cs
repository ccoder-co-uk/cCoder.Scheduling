using cCoder.Data;
using cCoder.Scheduling.Brokers.Events;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using EventLibrary.Models;


namespace cCoder.Scheduling.Services.Foundations.Events;

internal class CalendarEntityEventService(
    ICalendarEntityEventBroker calendarEventBroker,
    ICoreAuthInfo authInfo
) : ICalendarEntityEventService
{
    public async ValueTask RaiseCalendarAddEventAsync(Calendar entity)
    {
        EventMessage<cCoder.Data.Models.Planning.Calendar> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalCalendar(entity),
        };

        await calendarEventBroker.RaiseCalendarAddEventAsync(message);
    }

    public async ValueTask RaiseCalendarUpdateEventAsync(Calendar entity)
    {
        EventMessage<cCoder.Data.Models.Planning.Calendar> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalCalendar(entity),
        };

        await calendarEventBroker.RaiseCalendarUpdateEventAsync(message);
    }

    public async ValueTask RaiseCalendarDeleteEventAsync(Calendar entity)
    {
        EventMessage<cCoder.Data.Models.Planning.Calendar> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalCalendar(entity),
        };

        await calendarEventBroker.RaiseCalendarDeleteEventAsync(message);
    }

    private static cCoder.Data.Models.Planning.Calendar ToInternalCalendar(Calendar item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
            App = item.App == null ? null : ToExternalApp(item.App),
            Events = item.Events?.Select(ToExternalCalendarEvent).ToArray(),
        };

    static cCoder.Data.Models.CMS.App ToExternalApp(App item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
        };

    static cCoder.Data.Models.Planning.CalendarEvent ToExternalCalendarEvent(CalendarEvent item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Start = item.Start,
            DurationInTicks = item.DurationInTicks,
            CalendarId = item.CalendarId,
        };
}











