using cCoder.Scheduling.Brokers.Storage;
using cCoder.Scheduling.Services.Foundations;
using Moq;
using DataFlowDefinition = cCoder.Data.Models.Workflow.FlowDefinition;
using DataRole = cCoder.Data.Models.Security.Role;
using DataUser = cCoder.Data.Models.Security.User;
using DataUserRole = cCoder.Data.Models.Security.UserRole;


namespace cCoder.Scheduling.Tests;

public partial class FlowQueueServiceTests
{
    private readonly Mock<IFlowQueueBroker> brokerMock;
    private readonly FlowQueueService service;

    public FlowQueueServiceTests()
    {
        brokerMock = new Mock<IFlowQueueBroker>(MockBehavior.Strict);
        service = new FlowQueueService(brokerMock.Object);
    }

    private static DataFlowDefinition CreateDataFlowDefinition(Guid id = default) =>
        new()
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            AppId = 1,
            Name = "Flow",
            Description = "Description",
            DefinitionJson =
                "{\"Activities\":[{\"$type\":\"cCoder.Core.Objects.Workflow.Activities.Start, cCoder.Core.Objects\",\"Name\":\"Start\"}],\"Links\":[]}",
        };

    private static DataUser CreateDataUser(string id = "user-1") =>
        new()
        {
            Id = id,
            DisplayName = "User",
            Email = "user@example.test",
            Roles =
            [
                new DataUserRole
                {
                    RoleId = Guid.NewGuid(),
                    UserId = id,
                    Role = new DataRole
                    {
                        Id = Guid.NewGuid(),
                        AppId = 1,
                        Name = "Admins",
                        Description = "Admins",
                        Privs = "app_admin,flowdefinition_execute",
                    },
                },
            ],
        };
}



