using cCoder.Scheduling.Brokers.Storage;
using AuthorizationBroker = cCoder.Scheduling.Brokers.AuthorizationBroker;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Scheduling.Services.Foundations;
using FizzWare.NBuilder;
using Moq;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class ScheduledTaskServiceTests
{
    private readonly Mock<IScheduledTaskBroker> scheduledTaskBrokerMock;
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock;
    private readonly ScheduledTaskService scheduledTaskService;

    public ScheduledTaskServiceTests()
    {
        scheduledTaskBrokerMock = new Mock<IScheduledTaskBroker>(MockBehavior.Strict);
        authorizationBrokerMock = new Mock<IAuthorizationBroker>(MockBehavior.Strict);
        scheduledTaskService = new ScheduledTaskService(
            scheduledTaskBrokerMock.Object,
            authorizationBrokerMock.Object
        );
    }

    private static ScheduledTask CreateRandomScheduledTask(int id = 42, int appId = 7)
    {
        ScheduledTask scheduledTask = Builder<ScheduledTask>
            .CreateNew()
            .With(x => x.Id = id)
            .With(x => x.AppId = appId)
            .With(x => x.FlowId = Guid.NewGuid())
            .With(x => x.ExcludedEventsCalendarId = null)
            .With(x => x.ExcludedEventsName = "Holidays")
            .With(x => x.Name = $"ScheduledTask-{Guid.NewGuid():N}")
            .With(x => x.Description = $"Description-{Guid.NewGuid():N}")
            .With(x => x.ExecutionArgs = "{}")
            .With(x => x.ScheduleInTicks = TimeSpan.FromHours(1).Ticks)
            .With(x => x.CreatedBy = "tester")
            .With(x => x.UpdatedBy = "tester")
            .With(x => x.ExecuteAs = $"user-{Guid.NewGuid():N}")
            .With(x => x.Created = DateTimeOffset.UtcNow.AddMinutes(-10))
            .With(x => x.LastUpdated = DateTimeOffset.UtcNow)
            .With(x => x.NextExecution = DateTimeOffset.UtcNow.AddHours(1))
            .Build();

        return scheduledTask;
    }
}














