using Application.Features.Queries.GetProductById;
using Application.Repositories;
using Domain;
using FluentValidation.TestHelper;
using NSubstitute;

namespace Application.UnitTests.Features.Queries.GetProductByIdTests
{
    [TestClass]
    public class GetProductByIdValidator
    {
        private IProductsRepository _productsRepository;
        private GetProductById.Validator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _productsRepository = Substitute.For<IProductsRepository>();
            _validator = new GetProductById.Validator(_productsRepository);
        }

        [TestMethod]
        public async Task Should_have_error_when_Id_is_zero()
        {
            var query = new GetProductById.Query { Id = 0 };
            var result = await _validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public async Task Should_have_error_when_product_not_found()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Product)null);

            var query = new GetProductById.Query { Id = 1 };
            var result = await _validator.TestValidateAsync(query);
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Product not found");
        }

        [TestMethod]
        public async Task Should_pass_for_valid_query()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Product());

            var query = new GetProductById.Query { Id = 1 };
            var result = await _validator.TestValidateAsync(query);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}