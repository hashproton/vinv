using Application.Features.Commands.AddProductToCategory;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.AddProductToCategoryTests;

[TestClass]
public class AddProductToCategoryValidatorTests
{
    private readonly AddProductToCategory.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorWhenProductIdIsZero()
    {
        var command = new AddProductToCategory.Command { ProductId = 0, CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [TestMethod]
    public void ShouldHaveErrorWhenCategoryIdIsZero()
    {
        var command = new AddProductToCategory.Command { ProductId = 1, CategoryId = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [TestMethod]
    public void ShouldNotHaveErrorWhenCommandIsValid()
    {
        var command = new AddProductToCategory.Command { ProductId = 1, CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}