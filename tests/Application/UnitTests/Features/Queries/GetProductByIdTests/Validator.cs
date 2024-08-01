using Application.Features.Queries.GetProductById;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Queries.GetProductByIdTests
{
    [TestClass]
    public class GetProductByIdValidatorTests
    {
        private readonly GetProductById.Validator _validator = new();

        [TestMethod]
        public void ShouldHaveErrorWhenIdIsZero()
        {
            var query = new GetProductById.Query { Id = 0 };
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public void ShouldNotHaveErrorWhenIdIsPositive()
        {
            var query = new GetProductById.Query { Id = 1 };
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
