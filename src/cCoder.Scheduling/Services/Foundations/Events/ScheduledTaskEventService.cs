using cCoder.Data;
using cCoder.Scheduling.Brokers.Events;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using EventLibrary.Models;


namespace cCoder.Scheduling.Services.Foundations.Events;

internal class ScheduledTaskEventService(
    IScheduledTaskEventBroker scheduledTaskEventBroker,
    ICoreAuthInfo authInfo
) : IScheduledTaskEventService
{
    public async ValueTask RaiseScheduledTaskAddEventAsync(ScheduledTask entity)
    {
        EventMessage<cCoder.Data.Models.Planning.ScheduledTask> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalScheduledTask(entity),
        };

        await scheduledTaskEventBroker.RaiseScheduledTaskAddEventAsync(message);
    }

    public async ValueTask RaiseScheduledTaskUpdateEventAsync(ScheduledTask entity)
    {
        EventMessage<cCoder.Data.Models.Planning.ScheduledTask> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalScheduledTask(entity),
        };

        await scheduledTaskEventBroker.RaiseScheduledTaskUpdateEventAsync(message);
    }

    public async ValueTask RaiseScheduledTaskDeleteEventAsync(ScheduledTask entity)
    {
        EventMessage<cCoder.Data.Models.Planning.ScheduledTask> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalScheduledTask(entity),
        };

        await scheduledTaskEventBroker.RaiseScheduledTaskDeleteEventAsync(message);
    }

    public async ValueTask RaiseScheduledTaskExecuteEventAsync(ScheduledTask entity)
    {
        EventMessage<cCoder.Data.Models.Planning.ScheduledTask> message = new()
        {
            AuthInfo = new EventAuthInfo { SSOUserId = authInfo.SSOUserId },
            Data = ToInternalScheduledTask(entity),
        };

        await scheduledTaskEventBroker.RaiseScheduledTaskExecuteEventAsync(message);
    }

    private static cCoder.Data.Models.Planning.ScheduledTask ToInternalScheduledTask(
        ScheduledTask item
    ) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            FlowId = item.FlowId,
            ExcludedEventsCalendarId = item.ExcludedEventsCalendarId,
            ExcludedEventsName = item.ExcludedEventsName,
            Name = item.Name,
            Description = item.Description,
            ExecutionArgs = item.ExecutionArgs,
            ScheduleInTicks = item.ScheduleInTicks,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            ExecuteAs = item.ExecuteAs,
            Created = item.Created,
            LastUpdated = item.LastUpdated,
            LastExecuted = item.LastExecuted,
            NextExecution = item.NextExecution,
            ExecuteAsUser = item.ExecuteAsUser == null ? null : ToExternalUser(item.ExecuteAsUser),
            App = item.App == null ? null : ToExternalApp(item.App),
            Flow = item.Flow == null ? null : ToExternalFlowDefinition(item.Flow),
            ExcludedEventsCalendar = item.ExcludedEventsCalendar == null ? null : ToExternalCalendar(item.ExcludedEventsCalendar),
        };

    static cCoder.Data.Models.Security.User ToExternalUser(User item) =>
        new()
        {
            Id = item.Id,
            DisplayName = item.DisplayName,
            Email = item.Email,
        };

    static cCoder.Data.Models.CMS.App ToExternalApp(App item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
        };

    static cCoder.Data.Models.Workflow.FlowDefinition ToExternalFlowDefinition(FlowDefinition item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
        };

    static cCoder.Data.Models.Planning.Calendar ToExternalCalendar(Calendar item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
        };
}











