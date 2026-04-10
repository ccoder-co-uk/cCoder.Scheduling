using System.Security;
using cCoder.Scheduling.Models;
using cCoder.Data.Models.CMS;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using cCoder.Data.Models.Workflow;
using cCoder.Workflow.Activities;
using FluentAssertions;
using Moq;
using Xunit;
using cCoder.Workflow.Activities.Models;


namespace cCoder.Core.Services.Tests.Planning.Orchestrations;

public partial class FlowQueueOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldQueueFlowInstanceWhenUserCanExecute()
    {
        Guid id = Guid.NewGuid();
        string asUserId = "user-1";
        string args = "{}";
        Guid queuedId = Guid.NewGuid();

        FlowDefinition flowDefinition = new()
        {
            Id = id,
            AppId = 1,
            Name = "Flow",
            Description = "Description",
            DefinitionJson =
                "{\"Activities\":[{\"$type\":\"cCoder.Core.Objects.Workflow.Activities.Start, cCoder.Core.Objects\",\"Name\":\"Start\"}],\"Links\":[]}",
        };

        User user = new()
        {
            Id = asUserId,
            Roles =
            [
                new UserRole
                {
                    RoleId = Guid.NewGuid(),
                    UserId = asUserId,
                    Role = new Role
                    {
                        Id = Guid.NewGuid(),
                        AppId = 1,
                        Name = "Admins",
                        Privileges = ["flowdefinition_execute"],
                    },
                },
            ],
        };

        flowQueueServiceMock.Setup(x => x.GetFlowDefinition(id)).Returns(flowDefinition);
        flowQueueServiceMock.Setup(x => x.GetUser(asUserId)).Returns(user);
        jsonBrokerMock
            .Setup(x => x.ParseJson<Flow>(flowDefinition.DefinitionJson))
            .Returns(new Flow
            {
                Activities = [new Start { Ref = "Start" }],
                Links = [],
            });
        jsonBrokerMock.Setup(x => x.ParseJson(args)).Returns(new { Name = "Value" });
        jsonBrokerMock
            .Setup(x => x.Serialize(It.IsAny<WorkflowContext>()))
            .Returns("{\"ExecutionState\":\"Queued\"}");
        flowQueueServiceMock
            .Setup(x => x.AddAsync(It.IsAny<FlowInstanceData>()))
            .ReturnsAsync((FlowInstanceData entity) =>
            {
                entity.Id = queuedId;
                return entity;
            });

        Guid result = await orchestrationService.QueueAsync(id, asUserId, args);

        result.Should().Be(queuedId);
        flowQueueServiceMock.Verify(x => x.GetFlowDefinition(id), Times.Once);
        flowQueueServiceMock.Verify(x => x.GetUser(asUserId), Times.Once);
        flowQueueServiceMock.Verify(
            x => x.AddAsync(It.Is<FlowInstanceData>(entity =>
                entity.FlowDefinitionId == id
                && entity.Caller == asUserId
                && entity.State == "Queued"
                && entity.ContextString.Contains("\"ExecutionState\":\"Queued\""))),
            Times.Once);
        jsonBrokerMock.Verify(x => x.ParseJson<Flow>(flowDefinition.DefinitionJson), Times.Once);
        jsonBrokerMock.Verify(x => x.ParseJson(args), Times.Once);
        jsonBrokerMock.Verify(x => x.Serialize(It.IsAny<WorkflowContext>()), Times.Once);
        flowQueueServiceMock.VerifyNoOtherCalls();
        jsonBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserCannotExecute()
    {
        Guid id = Guid.NewGuid();
        string asUserId = "user-1";

        flowQueueServiceMock
            .Setup(x => x.GetFlowDefinition(id))
            .Returns(new FlowDefinition { Id = id, AppId = 1, DefinitionJson = "{}" });
        flowQueueServiceMock
            .Setup(x => x.GetUser(asUserId))
            .Returns(new User { Id = asUserId, Roles = [] });

        Func<Task> action = async () => await orchestrationService.QueueAsync(id, asUserId, "{}");

        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        flowQueueServiceMock.Verify(x => x.GetFlowDefinition(id), Times.Once);
        flowQueueServiceMock.Verify(x => x.GetUser(asUserId), Times.Once);
        flowQueueServiceMock.VerifyNoOtherCalls();
        jsonBrokerMock.VerifyNoOtherCalls();
    }
}

