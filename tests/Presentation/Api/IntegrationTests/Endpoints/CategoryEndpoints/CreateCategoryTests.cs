using Application.Features.Commands.CreateCategory;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Endpoints.CategoryEndpoints
{
    [TestClass]
    public class CreateCategoryTests : BaseIntegrationTest
    {
        [TestMethod]
        public async Task CreateCategory_ShouldReturnCreatedResult()
        {
            // Arrange
            var command = new CreateCategory.Command { Name = "Test Category" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/categories", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var id = await response.Content.ReadFromJsonAsync<int>();
            id.Should().BeGreaterThan(0);

            // Verify in the database
            var category = await Context.Categories.FindAsync(id);
            category.Should().NotBeNull();
            category!.Name.Should().Be("Test Category");
        }

        [TestMethod]
        public async Task CreateCategory_ShouldReturnBadRequest_WhenNameIsEmpty()
        {
            // Arrange
            var command = new CreateCategory.Command { Name = "" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/categories", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadAsStringAsync();
            error.Should().Contain("Name");
        }

        [TestMethod]
        public async Task CreateCategory_ShouldReturnBadRequest_WhenNameExceedsMaxLength()
        {
            // Arrange
            var command = new CreateCategory.Command { Name = new string('a', 201) };

            // Act
            var response = await Client.PostAsJsonAsync("/api/categories", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await response.Content.ReadAsStringAsync();
            error.Should().Contain("Name");
        }

        [TestMethod]
        public async Task CreateCategory_ShouldReturnConflict_WhenCategoryAlreadyExists()
        {
            // Arrange
            var existingCategory = new Domain.Category { Name = "Existing Category" };
            Context.Categories.Add(existingCategory);
            await Context.SaveChangesAsync();

            var command = new CreateCategory.Command { Name = "Existing Category" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/categories", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            var error = await response.Content.ReadAsStringAsync();
            error.Should().Contain("Category already exists");
        }
    }
}
