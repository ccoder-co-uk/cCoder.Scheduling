using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;

namespace cCoder.Scheduling.Services.Processings;

public interface ICalendarEventProcessingService
{
    CalendarEvent Get(int id);

    IQueryable<CalendarEvent> GetAll(bool ignoreFilters = false);

    ValueTask<CalendarEvent> AddAsync(CalendarEvent entity);

    ValueTask<CalendarEvent> UpdateAsync(CalendarEvent entity);

    ValueTask DeleteAsync(int id);

    ValueTask<IEnumerable<Result<CalendarEvent>>> AddOrUpdate(IEnumerable<CalendarEvent> items);

    ValueTask DeleteAllAsync(IEnumerable<CalendarEvent> items);
}
