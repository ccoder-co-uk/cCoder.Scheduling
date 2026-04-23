using System.Security;
using cCoder.Scheduling.Brokers.Storage;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using DataCalendarEvent = cCoder.Data.Models.Planning.CalendarEvent;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;


namespace cCoder.Scheduling.Services.Foundations;

internal class CalendarEventService(
    ICalendarEventBroker calendarEventBroker,
    IAuthorizationBroker authorizationBroker
) : ICalendarEventService
{
    public CalendarEvent Get(int id)
    {
        CalendarEvent calendarEvent = GetAll().FirstOrDefault(i => i.Id == id);
        if (calendarEvent is not null)
            return calendarEvent;

        CalendarEvent unrestrictedCalendarEvent = GetAll(true).FirstOrDefault(i => i.Id == id);
        if (unrestrictedCalendarEvent is not null)
            throw new SecurityException("Access Denied!");

        return null;
    }

    public IQueryable<CalendarEvent> GetAll(bool ignoreFilters = false) =>
        calendarEventBroker.GetAllCalendarEvents(ignoreFilters);

    public async ValueTask<CalendarEvent> AddAsync(CalendarEvent calendarEvent)
    {
        authorizationBroker.Authorize(
            calendarEventBroker.GetAppId(ToInternalCalendarEvent(calendarEvent)),
            $"{nameof(CalendarEvent)}_create"
        );
        DataCalendarEvent newCalendarEvent = new()
        {
            Name = calendarEvent.Name,
            Description = calendarEvent.Description,
            Start = calendarEvent.Start,
            DurationInTicks = calendarEvent.DurationInTicks,
            CalendarId = calendarEvent.CalendarId,
        };

        DataCalendarEvent result = await calendarEventBroker.AddCalendarEventAsync(newCalendarEvent);
        calendarEvent.Id = result.Id;
        calendarEvent.Name = result.Name;
        calendarEvent.Description = result.Description;
        calendarEvent.Start = result.Start;
        calendarEvent.DurationInTicks = result.DurationInTicks;
        calendarEvent.CalendarId = result.CalendarId;
        return calendarEvent;
    }

    public async ValueTask<CalendarEvent> UpdateAsync(CalendarEvent calendarEvent)
    {
        authorizationBroker.Authorize(
            calendarEventBroker.GetAppId(ToInternalCalendarEvent(calendarEvent)),
            $"{nameof(CalendarEvent)}_update"
        );
        DataCalendarEvent updateCalendarEvent = new()
        {
            Id = calendarEvent.Id,
            Name = calendarEvent.Name,
            Description = calendarEvent.Description,
            Start = calendarEvent.Start,
            DurationInTicks = calendarEvent.DurationInTicks,
            CalendarId = calendarEvent.CalendarId,
        };

        DataCalendarEvent result = await calendarEventBroker.UpdateCalendarEventAsync(
            updateCalendarEvent
        );
        calendarEvent.Id = result.Id;
        calendarEvent.Name = result.Name;
        calendarEvent.Description = result.Description;
        calendarEvent.Start = result.Start;
        calendarEvent.DurationInTicks = result.DurationInTicks;
        calendarEvent.CalendarId = result.CalendarId;
        return calendarEvent;
    }

    public async ValueTask DeleteAsync(int id)
    {
        CalendarEvent calendarEvent = GetAll(ignoreFilters: true).FirstOrDefault(item => item.Id == id);

        if (calendarEvent is null)
            return;

        authorizationBroker.Authorize(
            calendarEventBroker.GetAppId(ToInternalCalendarEvent(calendarEvent)),
            $"{nameof(CalendarEvent)}_delete"
        );
        _ = await calendarEventBroker.DeleteCalendarEventAsync(ToInternalCalendarEvent(calendarEvent));
    }

    private static CalendarEvent ToExternalCalendarEvent(
        DataCalendarEvent item,
        Calendar originalCalendar = null
    ) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Start = item.Start,
            DurationInTicks = item.DurationInTicks,
            CalendarId = item.CalendarId,
            Calendar = originalCalendar ?? (item.Calendar == null ? null : ToLocalCalendarShallow(item.Calendar)),
        };

    private static DataCalendarEvent ToInternalCalendarEvent(CalendarEvent item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Start = item.Start,
            DurationInTicks = item.DurationInTicks,
            CalendarId = item.CalendarId,
            Calendar = item.Calendar == null ? null : ToExternalCalendarShallow(item.Calendar),
        };

    static Calendar ToLocalCalendarShallow(cCoder.Data.Models.Planning.Calendar item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
        };

    static cCoder.Data.Models.Planning.Calendar ToExternalCalendarShallow(Calendar item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
        };
}














