using Application.Features.Queries.GetCategoryById;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.UnitTests.Features.Queries.GetCategoryByIdTests
{
    [TestClass]
    public class GetCategoryByIdValidatorTests
    {
        private GetCategoryById.Validator _validator;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new GetCategoryById.Validator();
        }

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
}