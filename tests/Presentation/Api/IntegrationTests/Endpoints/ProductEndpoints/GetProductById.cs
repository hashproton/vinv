using Application.Features.Queries.GetProductById;
using Application.Shared;
using FluentAssertions;
using Presentation.Api.IntegrationTests.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints.ProductEndpoints;

[TestClass]
public class GetProductByIdTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task GetProductById_ShouldReturnOk_WhenProductExists()
    {
        // Arrange
        var existingCategory = new Domain.Category { Name = "Existing Category" };
        await CategoriesRepository.CreateAsync(existingCategory, default);

        var existingProduct = new Domain.Product { Name = "Existing Product", CategoryId = existingCategory.Id };
        await ProductsRepository.CreateAsync(existingProduct, default);

        // Act
        var response = await Client.GetAsync($"/api/products/{existingProduct.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await response.Content.ReadFromJsonAsync<GetProductById.ProductDto>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(existingProduct.Id);
        product.Name.Should().Be(existingProduct.Name);
        product.CategoryId.Should().Be(existingProduct.CategoryId);
    }

    [TestMethod]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        const int nonExistentProductId = 999;

        // Act
        var response = await Client.GetAsync($"/api/products/{nonExistentProductId}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.NotFound, $"Product with id {nonExistentProductId} not found", ErrorType.NotFound);
    }

    [TestMethod]
    public async Task GetProductById_ShouldReturnBadRequest_WhenIdIsZero()
    {
        // Arrange
        var query = new GetProductById.Query { Id = 0 };

        // Act
        var response = await Client.GetAsync($"/api/products/{query.Id}");

        // Assert
        await response.ReadAndAssertProblemDetailsAsync(HttpStatusCode.BadRequest, "'Id' must be greater than '0'.", ErrorType.ValidationError);
    }
}
