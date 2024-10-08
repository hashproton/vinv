﻿using Application.Features.Commands.DeleteProduct;

namespace Presentation.Api.IntegrationTests.Endpoints.ProductEndpoints;

[TestClass]
public class DeleteProductTests : BaseIntegrationTest
{
    private const string Endpoint = "/api/products";

    [TestMethod]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenProductIsDeleted()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Product to Delete", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        // Act
        var response = await Client.DeleteAsync($"{Endpoint}/{existingProduct.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify in the database
        var deletedProduct = await ProductsRepository.GetByIdAsync(existingProduct.Id, default);
        deletedProduct.Should().BeNull();
    }

    [TestMethod]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentProductId = 999;

        // Act
        var response = await Client.DeleteAsync($"{Endpoint}/{nonExistentProductId}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Product with id {nonExistentProductId} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task DeleteProduct_ShouldReturnBadRequest_WhenIdIsZero()
    {
        // Arrange
        var command = new DeleteProduct.Command { Id = 0 };

        // Act
        var response = await Client.DeleteAsync($"{Endpoint}/{command.Id}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Id' must be greater than '0'.", ErrorType.ValidationError);
    }
}
