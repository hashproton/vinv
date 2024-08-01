using Application.Features.Commands.UpdateProduct;
using Application.Repositories;
using Domain;
using FluentValidation.TestHelper;
using NSubstitute;

namespace Application.UnitTests.Features.Commands.UpdateProductTests
{
    [TestClass]
    public class UpdateProductValidator
    {
        private IProductsRepository _productsRepository;
        private ICategoriesRepository _categoriesRepository;
        private UpdateProduct.Validator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _productsRepository = Substitute.For<IProductsRepository>();
            _categoriesRepository = Substitute.For<ICategoriesRepository>();
            _validator = new UpdateProduct.Validator(_productsRepository, _categoriesRepository);
        }

        [TestMethod]
        public async Task Should_have_error_when_Id_is_zero()
        {
            var command = new UpdateProduct.Command { Id = 0, Name = "Test", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public async Task Should_have_error_when_product_not_found()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Product)null);

            var command = new UpdateProduct.Command { Id = 1, Name = "Test", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Product not found");
        }

        [TestMethod]
        public async Task Should_not_have_error_when_product_found()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Product());

            var command = new UpdateProduct.Command { Id = 1, Name = "Test", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [TestMethod]
        public async Task Should_have_error_when_Name_is_empty()
        {
            var command = new UpdateProduct.Command { Id = 1, Name = "", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public async Task Should_have_error_when_Name_exceeds_maximum_length()
        {
            var command = new UpdateProduct.Command { Id = 1, Name = new string('a', 201), CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public async Task Should_have_error_when_CategoryId_is_zero()
        {
            var command = new UpdateProduct.Command { Id = 1, Name = "Test", CategoryId = 0 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        }

        [TestMethod]
        public async Task Should_have_error_when_category_not_found()
        {
            _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Category)null);

            var command = new UpdateProduct.Command { Id = 1, Name = "Test", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                .WithErrorMessage("Category not found");
        }

        [TestMethod]
        public async Task Should_not_have_error_when_category_found()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Product());
            _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Category());

            var command = new UpdateProduct.Command { Id = 1, Name = "Test", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
        }

        [TestMethod]
        public async Task Should_pass_for_valid_command()
        {
            _productsRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Product());
            _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Category());

            var command = new UpdateProduct.Command { Id = 1, Name = "Valid Name", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}