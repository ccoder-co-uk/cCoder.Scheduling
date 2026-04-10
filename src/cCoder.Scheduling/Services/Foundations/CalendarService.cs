using System.Security;
using cCoder.Scheduling.Brokers.Storage;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using DataCalendar = cCoder.Data.Models.Planning.Calendar;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;


namespace cCoder.Scheduling.Services.Foundations;

internal class CalendarService(
    ICalendarBroker calendarBroker,
    IAuthorizationBroker authorizationBroker
) : ICalendarService
{
    public Calendar Get(int id)
    {
        Calendar calendar = GetAll().FirstOrDefault(i => i.Id == id);
        if (calendar is not null)
            return calendar;

        Calendar unrestrictedCalendar = GetAll(true).FirstOrDefault(i => i.Id == id);
        if (unrestrictedCalendar is not null)
            throw new SecurityException("Access Denied!");

        return null;
    }

    public IQueryable<Calendar> GetAll(bool ignoreFilters = false) =>
        calendarBroker.GetAllCalendars(ignoreFilters);

    public async ValueTask<Calendar> AddAsync(Calendar calendar)
    {
        authorizationBroker.Authorize(calendar.AppId, $"{nameof(Calendar)}_create");
        DataCalendar newCalendar = new()
        {
            AppId = calendar.AppId,
            Name = calendar.Name,
            Description = calendar.Description,
        };

        newCalendar = await calendarBroker.AddCalendarAsync(newCalendar);
        return ToExternalCalendar(newCalendar, calendar.App, calendar.Events);
    }

    public async ValueTask<Calendar> UpdateAsync(Calendar calendar)
    {
        authorizationBroker.Authorize(calendar.AppId, $"{nameof(Calendar)}_update");
        DataCalendar updateCalendar = new()
        {
            Id = calendar.Id,
            AppId = calendar.AppId,
            Name = calendar.Name,
            Description = calendar.Description,
        };

        updateCalendar = await calendarBroker.UpdateCalendarAsync(updateCalendar);
        return ToExternalCalendar(updateCalendar, calendar.App, calendar.Events);
    }

    public async ValueTask DeleteAsync(int id)
    {
        Calendar calendar = GetAll(ignoreFilters: true).FirstOrDefault(item => item.Id == id);

        if (calendar is null)
            return;

        authorizationBroker.Authorize(calendar.AppId, $"{nameof(Calendar)}_delete");
        _ = await calendarBroker.DeleteCalendarAsync(ToInternalCalendar(calendar));
    }

    private static Calendar ToExternalCalendar(
        DataCalendar item,
        App originalApp = null,
        ICollection<CalendarEvent> originalEvents = null
    ) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
            App = originalApp ?? (item.App == null ? null : ToLocalApp(item.App)),
            Events = originalEvents ?? item.Events?.Select(ToExternalCalendarEvent).ToArray(),
        };

    private static CalendarEvent ToExternalCalendarEvent(cCoder.Data.Models.Planning.CalendarEvent item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Start = item.Start,
            DurationInTicks = item.DurationInTicks,
            CalendarId = item.CalendarId,
        };

    private static DataCalendar ToInternalCalendar(Calendar item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
            App = item.App == null ? null : ToExternalApp(item.App),
            Events = item.Events?.Select(calendarEvent => new cCoder.Data.Models.Planning.CalendarEvent
            {
                Id = calendarEvent.Id,
                Name = calendarEvent.Name,
                Description = calendarEvent.Description,
                Start = calendarEvent.Start,
                DurationInTicks = calendarEvent.DurationInTicks,
                CalendarId = calendarEvent.CalendarId,
            }).ToArray(),
        };

    static App ToLocalApp(cCoder.Data.Models.CMS.App item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
        };

    static cCoder.Data.Models.CMS.App ToExternalApp(App item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
        };
}














