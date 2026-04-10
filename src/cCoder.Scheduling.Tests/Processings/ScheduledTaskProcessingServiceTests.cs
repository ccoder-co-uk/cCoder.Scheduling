using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Scheduling.Services.Foundations;
using cCoder.Scheduling.Services.Processings;
using FizzWare.NBuilder;
using Moq;
using IAuthorizationBroker = cCoder.Scheduling.Brokers.IAuthorizationBroker;


namespace cCoder.Core.Services.Tests.Planning.Processings;

public partial class ScheduledTaskProcessingServiceTests
{
    private readonly Mock<IScheduledTaskService> scheduledTaskServiceMock = new();
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock = new();
    private readonly Mock<IScheduledTaskEventProcessingService> scheduledTaskEventProcessingServiceMock = new();
    private readonly ScheduledTaskProcessingService scheduledTaskProcessingService;

    public ScheduledTaskProcessingServiceTests()
    {
        scheduledTaskProcessingService = new ScheduledTaskProcessingService(
            scheduledTaskServiceMock.Object,
            authorizationBrokerMock.Object,
            scheduledTaskEventProcessingServiceMock.Object
        );
    }

    private static ScheduledTask CreateRandomScheduledTask() =>
        Builder<ScheduledTask>
            .CreateNew()
            .With(x => x.Id = Random.Shared.Next(1, 10000))
            .With(x => x.AppId = 1)
            .With(x => x.FlowId = Guid.NewGuid())
            .With(x => x.Name = $"Task-{Guid.NewGuid():N}")
            .With(x => x.ExecuteAs = "test-user")
            .With(x => x.Description = $"Description-{Guid.NewGuid():N}")
            .With(x => x.ExecutionArgs = "{}")
            .With(x => x.ExcludedEventsName = string.Empty)
            .With(x => x.ExecuteAsUser = null)
            .With(x => x.App = null)
            .With(x => x.Flow = null)
            .With(x => x.ExcludedEventsCalendar = null)
            .Build();
}














