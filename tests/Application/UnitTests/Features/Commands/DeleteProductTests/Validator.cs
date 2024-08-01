using Application.Features.Commands.DeleteProduct;
using Application.Repositories;
using Domain;
using FluentValidation.TestHelper;
using NSubstitute;

namespace Application.UnitTests.Features.Commands.DeleteProductTests
{
    [TestClass]
    public class DeleteProductValidator
    {
        private IProductsRepository _productsRepository;
        private DeleteProduct.Validator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _productsRepository = Substitute.For<IProductsRepository>();
            _validator = new DeleteProduct.Validator(_productsRepository);
        }

        [TestMethod]
        public async Task Should_have_error_when_Id_is_zero()
        {
            var command = new DeleteProduct.Command { Id = 0 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public async Task Should_have_error_when_product_not_found()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Product)null);

            var command = new DeleteProduct.Command { Id = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Product not found");
        }

        [TestMethod]
        public async Task Should_pass_for_valid_command()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Product());

            var command = new DeleteProduct.Command { Id = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}