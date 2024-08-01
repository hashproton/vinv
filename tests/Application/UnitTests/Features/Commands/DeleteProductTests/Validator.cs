using Application.Features.Commands.DeleteProduct;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Features.Commands.DeleteProductTests
{
    [TestClass]
    public class DeleteProductValidatorTests
    {
        private readonly DeleteProduct.Validator _validator = new();

        [TestMethod]
        public void ShouldHaveErrorWhenIdIsZero()
        {
            var command = new DeleteProduct.Command { Id = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public void ShouldNotHaveErrorWhenIdIsPositive()
        {
            var command = new DeleteProduct.Command { Id = 1 };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
