using Application.Features.Commands.UpdateProduct;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.UpdateProductTests;

[TestClass]
public class UpdateProductValidatorTests
{
    private readonly UpdateProduct.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var command = new UpdateProduct.Command { Id = 0, Name = "Valid Name", CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [TestMethod]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        var command = new UpdateProduct.Command { Id = 1, Name = "", CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void ShouldHaveErrorWhenNameExceedsMaxLength()
    {
        var command = new UpdateProduct.Command { Id = 1, Name = new string('a', 201), CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void ShouldHaveErrorWhenCategoryIdIsZero()
    {
        var command = new UpdateProduct.Command { Id = 1, Name = "Valid Product Name", CategoryId = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [TestMethod]
    public void ShouldNotHaveErrorWhenCommandIsValid()
    {
        var command = new UpdateProduct.Command { Id = 1, Name = "Valid Product Name", CategoryId = 1 };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}