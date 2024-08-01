using Application.Features.Commands.DeleteCategory;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.DeleteCategoryTests
{
    [TestClass]
    public class DeleteCategoryValidatorTests
    {
        private readonly DeleteCategory.Validator _validator = new();

        [TestMethod]
        public void ShouldHaveErrorWhenIdIsZero()
        {
            var command = new DeleteCategory.Command { Id = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public void ShouldNotHaveErrorWhenIdIsPositive()
        {
            var command = new DeleteCategory.Command { Id = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}