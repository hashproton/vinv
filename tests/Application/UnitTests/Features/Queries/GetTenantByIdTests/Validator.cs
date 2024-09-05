using Application.Features.Queries.GetTenantById;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Queries.GetTenantByIdTests;

[TestClass]
public class GetTenantByIdValidatorTests
{
    private readonly GetTenantById.Validator _validator = new();


    [TestMethod]
    public void ShouldHaveErrorForId_WhenIdIsZero()
    {
        // Arrange
        var query = new GetTenantById.Query { Id = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }

    [TestMethod]
    public void ShouldNotHaveValidationErrors_WhenQueryIsValid()
    {
        // Arrange
        var query = new GetTenantById.Query { Id = 1 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}