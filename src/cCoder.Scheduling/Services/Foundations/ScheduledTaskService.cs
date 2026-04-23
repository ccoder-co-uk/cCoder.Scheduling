using System.Security;
using cCoder.Scheduling.Brokers.Storage;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using DataScheduledTask = cCoder.Data.Models.Planning.ScheduledTask;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;


namespace cCoder.Scheduling.Services.Foundations;

internal class ScheduledTaskService(
    IScheduledTaskBroker scheduledTaskBroker,
    IAuthorizationBroker authorizationBroker
) : IScheduledTaskService
{
    public ScheduledTask Get(int id)
    {
        ScheduledTask scheduledTask = GetAll().FirstOrDefault(i => i.Id == id);
        if (scheduledTask is not null)
            return scheduledTask;

        ScheduledTask unrestrictedScheduledTask = GetAll(true).FirstOrDefault(i => i.Id == id);
        if (unrestrictedScheduledTask is not null)
            throw new SecurityException("Access Denied!");

        return null;
    }

    public ScheduledTask GetForExecution(int id) =>
        ToExternalScheduledTask(scheduledTaskBroker.GetScheduledTaskForExecution(id));

    public IQueryable<ScheduledTask> GetAll(bool ignoreFilters = false) =>
        scheduledTaskBroker.GetAllScheduledTasks(ignoreFilters);

    public async ValueTask<ScheduledTask> MarkExecutedAsync(int id, bool incrementNextExecution) =>
        ToExternalScheduledTask(
            await scheduledTaskBroker.MarkScheduledTaskExecutedAsync(id, incrementNextExecution)
        );

    public bool ExecuteAsUserBelongsToApp(string executeAs, int appId) =>
        scheduledTaskBroker.ExecuteAsUserBelongsToApp(executeAs, appId);

    public bool FlowBelongsToApp(Guid flowId, int appId) =>
        scheduledTaskBroker.FlowBelongsToApp(flowId, appId);

    public async ValueTask<ScheduledTask> AddAsync(ScheduledTask scheduledTask)
    {
        authorizationBroker.Authorize(scheduledTask.AppId, $"{nameof(ScheduledTask)}_create");
        DataScheduledTask newScheduledTask = new()
        {
            AppId = scheduledTask.AppId,
            FlowId = scheduledTask.FlowId,
            ExcludedEventsCalendarId = scheduledTask.ExcludedEventsCalendarId,
            ExcludedEventsName = scheduledTask.ExcludedEventsName,
            Name = scheduledTask.Name,
            Description = scheduledTask.Description,
            ExecutionArgs = scheduledTask.ExecutionArgs,
            ScheduleInTicks = scheduledTask.ScheduleInTicks,
            CreatedBy = scheduledTask.CreatedBy,
            UpdatedBy = scheduledTask.UpdatedBy,
            ExecuteAs = scheduledTask.ExecuteAs,
            Created = scheduledTask.Created,
            LastUpdated = scheduledTask.LastUpdated,
            LastExecuted = scheduledTask.LastExecuted,
            NextExecution = scheduledTask.NextExecution,
        };
        string currentUserId = authorizationBroker.GetCurrentUser().Id;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        newScheduledTask.Created = now;
        newScheduledTask.CreatedBy = currentUserId;
        newScheduledTask.LastUpdated = now;
        newScheduledTask.UpdatedBy = currentUserId;

        DataScheduledTask result = await scheduledTaskBroker.AddScheduledTaskAsync(newScheduledTask);
        scheduledTask.Id = result.Id;
        scheduledTask.AppId = result.AppId;
        scheduledTask.FlowId = result.FlowId;
        scheduledTask.ExcludedEventsCalendarId = result.ExcludedEventsCalendarId;
        scheduledTask.ExcludedEventsName = result.ExcludedEventsName;
        scheduledTask.Name = result.Name;
        scheduledTask.Description = result.Description;
        scheduledTask.ExecutionArgs = result.ExecutionArgs;
        scheduledTask.ScheduleInTicks = result.ScheduleInTicks;
        scheduledTask.CreatedBy = result.CreatedBy;
        scheduledTask.UpdatedBy = result.UpdatedBy;
        scheduledTask.ExecuteAs = result.ExecuteAs;
        scheduledTask.Created = result.Created;
        scheduledTask.LastUpdated = result.LastUpdated;
        scheduledTask.LastExecuted = result.LastExecuted;
        scheduledTask.NextExecution = result.NextExecution;
        return scheduledTask;
    }

    public async ValueTask<ScheduledTask> UpdateAsync(ScheduledTask scheduledTask)
    {
        authorizationBroker.Authorize(scheduledTask.AppId, $"{nameof(ScheduledTask)}_update");
        DataScheduledTask updateScheduledTask = new()
        {
            Id = scheduledTask.Id,
            AppId = scheduledTask.AppId,
            FlowId = scheduledTask.FlowId,
            ExcludedEventsCalendarId = scheduledTask.ExcludedEventsCalendarId,
            ExcludedEventsName = scheduledTask.ExcludedEventsName,
            Name = scheduledTask.Name,
            Description = scheduledTask.Description,
            ExecutionArgs = scheduledTask.ExecutionArgs,
            ScheduleInTicks = scheduledTask.ScheduleInTicks,
            CreatedBy = scheduledTask.CreatedBy,
            UpdatedBy = scheduledTask.UpdatedBy,
            ExecuteAs = scheduledTask.ExecuteAs,
            Created = scheduledTask.Created,
            LastUpdated = scheduledTask.LastUpdated,
            LastExecuted = scheduledTask.LastExecuted,
            NextExecution = scheduledTask.NextExecution,
        };
        string currentUserId = authorizationBroker.GetCurrentUser().Id;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        updateScheduledTask.LastUpdated = now;
        updateScheduledTask.UpdatedBy = currentUserId;

        DataScheduledTask result = await scheduledTaskBroker.UpdateScheduledTaskAsync(
            updateScheduledTask
        );
        scheduledTask.Id = result.Id;
        scheduledTask.AppId = result.AppId;
        scheduledTask.FlowId = result.FlowId;
        scheduledTask.ExcludedEventsCalendarId = result.ExcludedEventsCalendarId;
        scheduledTask.ExcludedEventsName = result.ExcludedEventsName;
        scheduledTask.Name = result.Name;
        scheduledTask.Description = result.Description;
        scheduledTask.ExecutionArgs = result.ExecutionArgs;
        scheduledTask.ScheduleInTicks = result.ScheduleInTicks;
        scheduledTask.CreatedBy = result.CreatedBy;
        scheduledTask.UpdatedBy = result.UpdatedBy;
        scheduledTask.ExecuteAs = result.ExecuteAs;
        scheduledTask.Created = result.Created;
        scheduledTask.LastUpdated = result.LastUpdated;
        scheduledTask.LastExecuted = result.LastExecuted;
        scheduledTask.NextExecution = result.NextExecution;
        return scheduledTask;
    }

    public async ValueTask DeleteAsync(int id)
    {
        ScheduledTask scheduledTask = GetAll(ignoreFilters: true).FirstOrDefault(item => item.Id == id);

        if (scheduledTask is null)
            return;

        authorizationBroker.Authorize(scheduledTask.AppId, $"{nameof(ScheduledTask)}_delete");
        _ = await scheduledTaskBroker.DeleteScheduledTaskAsync(ToInternalScheduledTask(scheduledTask));
    }

    private static ScheduledTask ToExternalScheduledTask(
        DataScheduledTask item,
        User originalExecuteAsUser = null,
        App originalApp = null,
        FlowDefinition originalFlow = null,
        Calendar originalExcludedEventsCalendar = null
    ) =>
        item == null
            ? null
            :
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
            ExecuteAsUser = originalExecuteAsUser ?? (item.ExecuteAsUser == null ? null : ToLocalUser(item.ExecuteAsUser)),
            App = originalApp ?? (item.App == null ? null : ToLocalApp(item.App)),
            Flow = originalFlow ?? (item.Flow == null ? null : ToLocalFlowDefinition(item.Flow)),
            ExcludedEventsCalendar = originalExcludedEventsCalendar
                ?? (item.ExcludedEventsCalendar == null ? null : ToLocalCalendarShallow(item.ExcludedEventsCalendar)),
        };

    private static DataScheduledTask ToInternalScheduledTask(ScheduledTask item) =>
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
            ExcludedEventsCalendar = item.ExcludedEventsCalendar == null ? null : ToExternalCalendarShallow(item.ExcludedEventsCalendar),
        };

    static User ToLocalUser(cCoder.Data.Models.Security.User item) =>
        new()
        {
            Id = item.Id,
            DisplayName = item.DisplayName,
            Email = item.Email,
        };

    static cCoder.Data.Models.Security.User ToExternalUser(User item) =>
        new()
        {
            Id = item.Id,
            DisplayName = item.DisplayName,
            Email = item.Email,
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

    static FlowDefinition ToLocalFlowDefinition(cCoder.Data.Models.Workflow.FlowDefinition item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
        };

    static cCoder.Data.Models.Workflow.FlowDefinition ToExternalFlowDefinition(FlowDefinition item) =>
        new()
        {
            Id = item.Id,
            AppId = item.AppId,
            Name = item.Name,
            Description = item.Description,
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














