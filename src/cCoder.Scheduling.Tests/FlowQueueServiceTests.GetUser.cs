using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Scheduling.Tests;

public partial class FlowQueueServiceTests
{
    [Fact]
    public void ShouldMapUserAndRolesWhenGetUser()
    {
        const string id = "user-1";
        var dataUser = CreateDataUser(id);

        brokerMock.Setup(x => x.GetUser(id)).Returns(dataUser);

        var result = service.GetUser(id);

        result.Id.Should().Be(dataUser.Id);
        result.DisplayName.Should().Be(dataUser.DisplayName);
        result.Email.Should().Be(dataUser.Email);
        result.Roles.Should().HaveCount(1);
        result.Roles.First().Role.AppId.Should().Be(1);
        result.Roles.First().Role.Privileges.Should().Contain(["app_admin", "flowdefinition_execute"]);
        brokerMock.Verify(x => x.GetUser(id), Times.Once);
        brokerMock.VerifyNoOtherCalls();
    }
}

