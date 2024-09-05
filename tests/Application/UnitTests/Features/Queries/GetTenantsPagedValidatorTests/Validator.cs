using Application.Features.Queries.GetTenantsPaged;
using Application.Repositories.Shared;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Queries.GetTenantsPagedValidatorTests;

[TestClass]
public class GetTenantsPagedValidatorTests
{
    private readonly GetTenantsPaged.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorForSkip_WhenSkipIsNegative()
    {
        // Arrange
        var query = new GetTenantsPaged.Query
        {
            PagedQuery = new PagedQuery { Skip = -1, Take = 10 }
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PagedQuery.Skip);
    }

    [TestMethod]
    public void ShouldHaveErrorForTake_WhenTakeIsZero()
    {
        // Arrange
        var query = new GetTenantsPaged.Query
        {
            PagedQuery = new PagedQuery { Skip = 0, Take = 0 }
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PagedQuery.Take);
    }

    [TestMethod]
    public void ShouldHaveErrorForTake_WhenTakeIsNegative()
    {
        // Arrange
        var query = new GetTenantsPaged.Query
        {
            PagedQuery = new PagedQuery { Skip = 0, Take = -1 }
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PagedQuery.Take);
    }

    [TestMethod]
    public void ShouldNotHaveValidationErrors_WhenQueryIsValid()
    {
        // Arrange
        var query = new GetTenantsPaged.Query
        {
            PagedQuery = new PagedQuery { Skip = 0, Take = 10 }
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}