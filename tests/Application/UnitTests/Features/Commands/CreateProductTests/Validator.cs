using Application.Features.Commands.CreateProduct;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.CreateProductTests
{
    [TestClass]
    public class CreateProductValidatorTests
    {
        private readonly CreateProduct.Validator _validator = new();

        [TestMethod]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var command = new CreateProduct.Command { Name = "", CategoryId = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public void ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            var command = new CreateProduct.Command { Name = new string('a', 201), CategoryId = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public void ShouldHaveErrorWhenCategoryIdIsZero()
        {
            var command = new CreateProduct.Command { Name = "Valid Product Name", CategoryId = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        }

        [TestMethod]
        public void ShouldNotHaveErrorWhenCommandIsValid()
        {
            var command = new CreateProduct.Command { Name = "Valid Product Name", CategoryId = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
