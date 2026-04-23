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
    public async Task ShouldDelegateToBrokerWhenUserIsAuthorizedForAddAsync()
    {
        // Given
        authorizationBrokerMock.Setup(x => x.GetCurrentUser()).Returns(new User { Id = "test-user" });
        ScheduledTask scheduledTask = CreateRandomScheduledTask(appId: 7);

        ScheduledTask submitted = null;

        scheduledTaskBrokerMock.Setup(x => x.GetAppId(It.IsAny<ScheduledTask>())).Returns((int?)7);

        authorizationBrokerMock.Setup(x => x.Authorize((int?)7, "ScheduledTask_create"));

        scheduledTaskBrokerMock
            .Setup(x =>
                x.AddScheduledTaskAsync(
                    It.Is<ScheduledTask>(candidate => !ReferenceEquals(candidate, scheduledTask))
                )
            )
            .Callback<ScheduledTask>(candidate => submitted = candidate)
            .ReturnsAsync((ScheduledTask value) => value);

        // When
        ScheduledTask result = await scheduledTaskService.AddAsync(scheduledTask);

        // Then
        result.Should().BeSameAs(scheduledTask);
        submitted.Should().NotBeNull();
        submitted.Should().NotBeSameAs(scheduledTask);
        result.Should().NotBeSameAs(submitted);

        submitted
            .Should()
            .BeEquivalentTo(
                scheduledTask,
                options =>
                    options
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("CreatedOn")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("CreatedBy")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("LastUpdated")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("LastUpdatedBy")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("LastUpdatedOn")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("UpdatedBy")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("Created")
                        )
                        .Excluding(candidate => candidate.Id)
            );

        result
            .Should()
            .BeEquivalentTo(
                scheduledTask,
                options =>
                    options
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("CreatedOn")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("CreatedBy")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("LastUpdated")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("LastUpdatedBy")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("LastUpdatedOn")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("UpdatedBy")
                        )
                        .Excluding(
                            (FluentAssertions.Equivalency.IMemberInfo info) =>
                                info.Path.EndsWith("Created")
                        )
                        .Excluding(candidate => candidate.Id)
            );

        scheduledTaskBrokerMock.Verify(
            x =>
                x.AddScheduledTaskAsync(
                    It.Is<ScheduledTask>(candidate => !ReferenceEquals(candidate, scheduledTask))
                ),
            Times.Once
        );
        scheduledTaskBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<ScheduledTask>()),
            Times.AtMostOnce()
        );
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "ScheduledTask_create"),
            Times.Once
        );
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserLacksCreatePrivilegeForAddAsync()
    {
        // Given
        ScheduledTask scheduledTask = CreateRandomScheduledTask(appId: 7);

        authorizationBrokerMock
            .Setup(x => x.Authorize((int?)7, "ScheduledTask_create"))
            .Throws(new SecurityException("Access Denied!"));

        // When
        Func<Task> action = async () => await scheduledTaskService.AddAsync(scheduledTask);

        // Then
        await action.Should().ThrowAsync<SecurityException>().WithMessage("Access Denied!");
        scheduledTaskBrokerMock.Verify(
            x => x.GetAppId(It.IsAny<ScheduledTask>()),
            Times.AtMostOnce()
        );
        scheduledTaskBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.Verify(
            x => x.Authorize((int?)7, "ScheduledTask_create"),
            Times.Once
        );
    }

}













