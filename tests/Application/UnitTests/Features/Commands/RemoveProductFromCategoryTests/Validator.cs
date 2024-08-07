using Application.Features.Commands.RemoveProductFromCategory;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.RemoveProductFromCategoryTests;

[TestClass]
public class RemoveProductFromCategoryValidatorTests
{
    private readonly RemoveProductFromCategory.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorWhenProductIdIsZero()
    {
        var command = new RemoveProductFromCategory.Command { ProductId = 0, CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [TestMethod]
    public void ShouldHaveErrorWhenCategoryIdIsZero()
    {
        var command = new RemoveProductFromCategory.Command { ProductId = 1, CategoryId = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [TestMethod]
    public void ShouldNotHaveErrorWhenCommandIsValid()
    {
        var command = new RemoveProductFromCategory.Command { ProductId = 1, CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}