using Application.Features.Commands.CreateProduct;
using Application.Repositories;
using Domain;
using FluentValidation.TestHelper;
using NSubstitute;

namespace Application.UnitTests.Features.Commands.CreateProductTests
{
    [TestClass]
    public class CreateProductValidator
    {
        private ICategoriesRepository _categoriesRepository;
        private CreateProduct.Validator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _categoriesRepository = Substitute.For<ICategoriesRepository>();
            _validator = new CreateProduct.Validator(_categoriesRepository);
        }

        [TestMethod]
        public async Task Should_have_error_when_Name_is_empty()
        {
            var command = new CreateProduct.Command { Name = "", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public async Task Should_have_error_when_Name_exceeds_maximum_length()
        {
            var command = new CreateProduct.Command { Name = new string('a', 201), CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [TestMethod]
        public async Task Should_have_error_when_CategoryId_is_zero()
        {
            var command = new CreateProduct.Command { Name = "Test", CategoryId = 0 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        }

        [TestMethod]
        public async Task Should_have_error_when_category_not_found()
        {
            _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Category)null);

            var command = new CreateProduct.Command { Name = "Test", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                .WithErrorMessage("Category not found");
        }

        [TestMethod]
        public async Task Should_pass_for_valid_command()
        {
            _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(new Category());

            var command = new CreateProduct.Command { Name = "Valid Name", CategoryId = 1 };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}