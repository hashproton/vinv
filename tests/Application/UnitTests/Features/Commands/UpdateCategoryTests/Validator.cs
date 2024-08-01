using Application.Features.Commands.UpdateCategory;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.UnitTests.Features.Commands.UpdateCategoryTests
{
    [TestClass]
    public class UpdateCategoryValidatorTests
    {
        private UpdateCategory.Validator _validator;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new UpdateCategory.Validator();
        }

        [TestMethod]
        public void ShouldHaveErrorWhenIdIsZero()
        {
            var command = new UpdateCategory.Command { Id = 0, Name = "Valid Name" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var command = new UpdateCategory.Command { Id = 1, Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public void ShouldHaveErrorWhenNameExceedsMaxLength()
        {
            var command = new UpdateCategory.Command { Id = 1, Name = new string('a', 201) };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public void ShouldNotHaveErrorWhenCommandIsValid()
        {
            var command = new UpdateCategory.Command { Id = 1, Name = "Valid Category Name" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}