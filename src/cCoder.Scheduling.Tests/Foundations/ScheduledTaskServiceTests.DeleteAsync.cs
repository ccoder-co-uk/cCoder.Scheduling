using System.Security;
using cCoder.Data.Models.Planning;
using cCoder.Data.Models.Security;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Core.Services.Tests.Planning.Foundations;

public partial class ScheduledTaskServiceTests
{
    [Fact]
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForDeleteAsync()
    {
        // Given
        authorizationBrokerMock.Setup(x => x.GetCurrentUser()).Returns(new User { Id = "test-user" });
        ScheduledTask scheduledTask = CreateRandomScheduledTask(id: 9, appId: 7);

        scheduledTaskBrokerMock.Setup(x => x.GetAllScheduledTasks(true)).Returns(new[] { scheduledTask }.AsQueryable());

        scheduledTaskBrokerMock.Setup(x => x.GetAppId(It.IsAny<ScheduledTask>())).Returns((int?)7);
        authorizationBrokerMock.Setup(x => x.Authorize((int?)7, "ScheduledTask_delete"));

        scheduledTaskBrokerMock
            .Setup(
                x =>
                    x.DeleteScheduledTaskAsync(
                        It.Is<ScheduledTask>(candidate => candidate.Id == scheduledTask.Id)
                    )
            )
            .ReturnsAsync(1);

        // When
        await scheduledTaskService.DeleteAsync(9);

        // Then
        scheduledTaskBrokerMock.Verify(x => x.GetAllScheduledTasks(true), Times.Once);
        scheduledTaskBrokerMock.Verify(
            x =>
                x.DeleteScheduledTaskAsync(
                    It.Is<ScheduledTask>(candidate => candidate.Id == scheduledTask.Id)
                ),
            Times.Once
        );
        scheduledTaskBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<ScheduledTask>()),
            Times.AtMostOnce()
        );
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "ScheduledTask_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksDeletePrivilegeForDeleteAsync()
    {
        // Given
        ScheduledTask scheduledTask = CreateRandomScheduledTask(id: 9, appId: 7);

        scheduledTaskBrokerMock.Setup(x => x.GetAllScheduledTasks(true)).Returns(new[] { scheduledTask }.AsQueryable());

        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)7, "ScheduledTask_delete"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await scheduledTaskService.DeleteAsync(9);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        scheduledTaskBrokerMock.Verify(x => x.GetAllScheduledTasks(true), Times.Once);
        scheduledTaskBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<ScheduledTask>()),
            Times.AtMostOnce()
        );
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "ScheduledTask_delete"),
            Times.Once
        );
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

}












