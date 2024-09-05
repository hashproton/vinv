using Application.Features.Commands.UpdateTenant;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.UpdateTenantTests;

[TestClass]
public class UpdateTenantValidatorTests
{
    private readonly UpdateTenant.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorForId_WhenIdIsZero()
    {
        // Arrange
        var command = new UpdateTenant.Command
        {
            Id = 0,
            Name = "Valid Name",
            Status = Domain.TenantStatus.Active
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [TestMethod]
    public void ShouldHaveErrorForName_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateTenant.Command
        {
            Id = 1, 
            Name = string.Empty,
            Status = Domain.TenantStatus.Active
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void ShouldHaveErrorForName_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new UpdateTenant.Command
        {
            Id = 1,
            Name = new string('a', 201),
            Status = Domain.TenantStatus.Active
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(99)]
    [DataRow(1000)]
    public void ShouldHaveErrorForStatus_WhenStatusIsOutOfEnumRange(int invalidStatus)
    {
        // Arrange
        var command = new UpdateTenant.Command
        {
            Id = 1,
            Name = "Valid Name",
            Status = (Domain.TenantStatus)invalidStatus
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [TestMethod]
    public void ShouldNotHaveValidationErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateTenant.Command
        {
            Id = 1,
            Name = "Valid Name",
            Status = Domain.TenantStatus.Active
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}