using Application.Features.Queries.GetCategoryById;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Queries.GetCategoryByIdTests;

[TestClass]
public class GetCategoryByIdValidatorTests
{
    private readonly GetCategoryById.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var query = new GetCategoryById.Query { Id = 0 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [TestMethod]
    public void ShouldNotHaveErrorWhenIdIsPositive()
    {
        var query = new GetCategoryById.Query { Id = 1 };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}