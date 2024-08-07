using Application.Features.Commands.UpdateProduct;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints.ProductEndpoints;

[TestClass]
public class UpdateProductTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task UpdateProduct_ShouldReturnNoContent_WhenProductIsUpdated()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Existing Product", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        var command = new UpdateProduct.Command { Id = existingProduct.Id, Name = "Updated Product", CategoryId = existingCategory.Id };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify in the database
        var updatedProduct = await ProductsRepository.GetByIdAsync(existingProduct.Id, default);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be("Updated Product");
        updatedProduct.CategoryId.Should().Be(existingCategory.Id);
    }

    [TestMethod]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new UpdateProduct.Command { Id = 999, Name = "Nonexistent Product", CategoryId = 1 };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Product with id {command.Id} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var category = new Domain.Category { Name = "Category" };
        await CategoriesRepository.CreateAsync(category, default);
        var existingProduct = new Domain.Product { Name = "Existing Product", CategoryId = category.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        var command = new UpdateProduct.Command { Id = existingProduct.Id, Name = "Updated Product", CategoryId = 999 };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Category with id {command.CategoryId} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenIdIsZero()
    {
        // Arrange
        var command = new UpdateProduct.Command { Id = 0, Name = "Updated Product", CategoryId = 1 };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Id' must be greater than '0'.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Existing Product", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        var command = new UpdateProduct.Command { Id = existingProduct.Id, Name = "", CategoryId = existingCategory.Id };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Name' must not be empty.", ErrorType.ValidationError);
    }

    [TestMethod]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenNameExceedsMaxLength()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Existing Product", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        var command = new UpdateProduct.Command { Id = existingProduct.Id, Name = new string('a', 201), CategoryId = existingCategory.Id };

        // Act
        var response = await Client.PutAsJsonAsync("/api/products", command);

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "The length of 'Name' must be 200 characters or fewer. You entered 201 characters.", ErrorType.ValidationError);
    }
}
