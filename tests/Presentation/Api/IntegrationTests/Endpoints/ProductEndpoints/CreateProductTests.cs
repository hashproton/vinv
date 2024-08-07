using Application.Features.Commands.CreateProduct;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints.ProductEndpoints;

[TestClass]
public class CreateProductTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task CreateProduct_ShouldReturnCreatedResult_WhenProductIsCreated()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var command = new CreateProduct.Command { Name = "New Product", CategoryId = existingCategory.Id };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var productId = await response.Content.ReadFromJsonAsync<int>();
        productId.Should().BeGreaterThan(0);

        // Verify in the database
        var product = await ProductsRepository.GetByIdAsync(productId, default);
        product.Should().NotBeNull();
        product!.Name.Should().Be("New Product");
        product.CategoryId.Should().Be(existingCategory.Id);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateProduct.Command { Name = "", CategoryId = 1 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Name' must not be empty.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new CreateProduct.Command { Name = new string('a', 201), CategoryId = 1 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "The length of 'Name' must be 200 characters or fewer. You entered 201 characters.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenCategoryIdIsZero()
    {
        // Arrange
        var command = new CreateProduct.Command { Name = "Valid Product Name", CategoryId = 0 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Category Id' must be greater than '0'.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new CreateProduct.Command { Name = "New Product", CategoryId = 999 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Category with id {command.CategoryId} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task CreateProduct_ShouldReturnConflict_WhenProductAlreadyExists()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Existing Product", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        var command = new CreateProduct.Command { Name = "Existing Product", CategoryId = existingCategory.Id };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.Conflict, $"Product with name {command.Name} already exists", ErrorType.AlreadyExists);
    }
}
