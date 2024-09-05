using Application.Features.Commands.DeleteTenant;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.DeleteTenantTests;

[TestClass]
public class DeleteTenantValidatorTests
{
    private readonly DeleteTenant.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorForId_WhenIdIsZero()
    {
        // Arrange
        var command = new DeleteTenant.Command { Id = 0 };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [TestMethod]
    public void ShouldNotHaveValidationErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeleteTenant.Command { Id = 1 };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}