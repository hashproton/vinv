using Application.Features.Commands.CreateCategory;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.CreateCategoryTests;

[TestClass]
public class CreateCategoryValidatorTests
{
    private readonly CreateCategory.Validator _validator = new();

    [TestMethod]
    public void ShouldHaveErrorWhenNameIsEmpty()
    {
        var command = new CreateCategory.Command { Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void ShouldHaveErrorWhenNameExceedsMaxLength()
    {
        var command = new CreateCategory.Command { Name = new string('a', 201) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void ShouldNotHaveErrorWhenNameIsValid()
    {
        var command = new CreateCategory.Command { Name = "Valid Category Name" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}