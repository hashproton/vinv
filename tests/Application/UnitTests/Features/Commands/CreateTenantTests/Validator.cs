using Application.Features.Commands.CreateTenant;
using Domain;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.CreateTenantTests;

[TestClass]
public class CreateTenantValidatorTests
{
    private readonly CreateTenant.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorForName_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateTenant.Command { Name = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [TestMethod]
    public void ShouldHaveErrorForName_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new CreateTenant.Command { Name = new string('a', 201) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(99)]
    [DataRow(1000)]
    public void ShouldHaveErrorForStatus_WhenStatusIsOutOfEnumRange(int invalidStatus)
    {
        // Arrange
        var command = new CreateTenant.Command
        {
            Name = "Valid Name",
            Status = (TenantStatus)invalidStatus
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Status);
    }

    [TestMethod]
    public void ShouldNotHaveValidationErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateTenant.Command { Name = "Valid Tenant Name" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}