using Application.Features.Commands.CreateTenant;
using Domain;
using FluentAssertions;

namespace Application.UnitTests.Features.Commands.CreateTenantTests;

[TestClass]
public class CreateTenantTestsCommand
{
    [TestMethod]
    public void ShouldSetDefaultStatusToInactive_WhenStatusIsNotProvided()
    {
        // Arrange
        var command = new CreateTenant.Command
        {
            Name = "Valid Tenant Name"
        };

        // Act & Assert
        command.Status.Should().Be(TenantStatus.Inactive);
    }
}