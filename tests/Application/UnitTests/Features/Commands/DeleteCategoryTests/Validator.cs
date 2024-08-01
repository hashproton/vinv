using Application.Features.Commands.DeleteCategory;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.UnitTests.Features.Commands.DeleteCategoryTests
{
    [TestClass]
    public class DeleteCategoryValidatorTests
    {
        private DeleteCategory.Validator _validator;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new DeleteCategory.Validator();
        }

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