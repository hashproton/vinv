using Application.Features.Commands.CreateCategory;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.UnitTests.Features.Commands.CreateCategoryTests
{
    [TestClass]
    public class CreateCategoryValidatorTests
    {
        private CreateCategory.Validator _validator;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new CreateCategory.Validator();
        }

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
}