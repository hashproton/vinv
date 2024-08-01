using Application.Features.Commands.CreateCategory;
using Application.Features.Commands.UpdateCategory;
using Application.Features.Queries.GetCategoryById;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints;

[TestClass]
public class CategoryEndpointsTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task CreateCategory_ReturnsCreatedResult()
    {
        // Arrange
        var command = new CreateCategory.Command { Name = "Test Category" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/categories", command);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var id = await response.Content.ReadFromJsonAsync<int>();
        Assert.IsNotNull(id);
    }

    [TestMethod]
    public async Task GetCategoryById_ReturnsCategory()
    {
        // Arrange
        var createCommand = new CreateCategory.Command { Name = "Test Category" };
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createCommand);
        var id = await createResponse.Content.ReadFromJsonAsync<int>();

        // Act
        var response = await Client.GetAsync($"/api/categories/{id}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var category = await response.Content.ReadFromJsonAsync<GetCategoryById.CategoryDto>();
        Assert.IsNotNull(category);
        Assert.AreEqual("Test Category", category.Name);
    }

    [TestMethod]
    public async Task UpdateCategory_ReturnsNoContent()
    {
        // Arrange
        var createCommand = new CreateCategory.Command { Name = "Test Category" };
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createCommand);
        var id = await createResponse.Content.ReadFromJsonAsync<int>();

        var updateCommand = new UpdateCategory.Command { Id = id, Name = "Updated Category" };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/categories/{id}", updateCommand);

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task DeleteCategory_ReturnsNoContent()
    {
        // Arrange
        var createCommand = new CreateCategory.Command { Name = "Test Category" };
        var createResponse = await Client.PostAsJsonAsync("/api/categories", createCommand);
        var id = await createResponse.Content.ReadFromJsonAsync<int>();

        // Act
        var response = await Client.DeleteAsync($"/api/categories/{id}");

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }
}