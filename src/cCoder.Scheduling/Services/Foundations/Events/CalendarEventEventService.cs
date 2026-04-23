using cCoder.Data;
using cCoder.Scheduling.Brokers.Events;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Eventing.Models;


namespace cCoder.Scheduling.Services.Foundations.Events;

internal class CalendarEventEventService(
    ICalendarEventEventBroker calendarEventEventBroker,
    ICoreAuthInfo authInfo
) : ICalendarEventEventService
{
    public async ValueTask RaiseCalendarEventAddEventAsync(CalendarEvent entity)
    {
        EventMessage<cCoder.Data.Models.Planning.CalendarEvent> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalCalendarEvent(entity),
        };

        await calendarEventEventBroker.RaiseCalendarEventAddEventAsync(message);
    }

    public async ValueTask RaiseCalendarEventUpdateEventAsync(CalendarEvent entity)
    {
        EventMessage<cCoder.Data.Models.Planning.CalendarEvent> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalCalendarEvent(entity),
        };

        await calendarEventEventBroker.RaiseCalendarEventUpdateEventAsync(message);
    }

    public async ValueTask RaiseCalendarEventDeleteEventAsync(CalendarEvent entity)
    {
        EventMessage<cCoder.Data.Models.Planning.CalendarEvent> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalCalendarEvent(entity),
        };

        await calendarEventEventBroker.RaiseCalendarEventDeleteEventAsync(message);
    }

    private static cCoder.Data.Models.Planning.CalendarEvent ToInternalCalendarEvent(
        CalendarEvent item
    ) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Start = item.Start,
            DurationInTicks = item.DurationInTicks,
            CalendarId = item.CalendarId,
            Calendar = item.Calendar == null ? null : new cCoder.Data.Models.Planning.Calendar
            {
                Id = item.Calendar.Id,
                AppId = item.Calendar.AppId,
                Name = item.Calendar.Name,
                Description = item.Calendar.Description,
            },
        };
}











