using System.Net.Mime;
using Application.Features.Commands.AddProductToCategory;
using Application.Features.Commands.CreateCategory;
using Application.Features.Commands.DeleteCategory;
using Application.Features.Commands.RemoveProductFromCategory;
using Application.Features.Commands.UpdateCategory;
using Application.Features.Queries.GetCategoryById;
using Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Api.Extensions;

namespace Presentation.Api.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories").WithTags("Categories");

        group.MapPost("/", CreateCategory)
            .WithName("CreateCategory")
            .WithSummary("Creates a new category.")
            .WithDescription("Creates a new category in the system.")
            .Accepts<CreateCategory.Command>(MediaTypeNames.Application.Json)
            .Produces<Result<int>>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id:int}", GetCategoryById)
            .WithName("GetCategoryById")
            .WithSummary("Gets a category by ID.")
            .WithDescription("Retrieves a category by its unique ID.")
            .Produces<Result<GetCategoryById.CategoryDto>>()
            .Produces(StatusCodes.Status404NotFound);

        // group.MapGet("/", GetCategories)
        //     .WithName("GetCategories")
        //     .WithSummary("Gets all categories.")
        //     .WithDescription("Retrieves all categories in the system.")
        //     .Produces<Result<PageResult<GetCategories.CategoryDto>>>()
        //     .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/", UpdateCategory)
            .WithName("UpdateCategory")
            .WithSummary("Updates an existing category.")
            .WithDescription("Updates an existing category's details.")
            .Accepts<UpdateCategory.Command>(MediaTypeNames.Application.Json)
            .Produces<Result>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteCategory)
            .WithName("DeleteCategory")
            .WithSummary("Deletes a category by ID.")
            .WithDescription("Deletes a category from the system using its unique ID.")
            .Produces<Result>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/{categoryId:int}/products/{productId:int}", AddProductToCategory)
            .WithName("AddProductToCategory")
            .WithSummary("Adds a product to a category.")
            .WithDescription("Adds an existing product to an existing category.")
            .Produces<Result>(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{categoryId:int}/products/{productId:int}", RemoveProductFromCategory)
            .WithName("RemoveProductFromCategory")
            .WithSummary("Removes a product from a category.")
            .WithDescription("Removes an existing product from an existing category.")
            .Produces<Result>(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> CreateCategory(IMediator mediator, [FromBody] CreateCategory.Command command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess
            ? Results.Created($"/api/categories/{result.Value}", result.Value)
            : result.MapError();
    }

    private static async Task<IResult> GetCategoryById(IMediator mediator, [FromRoute] int id)
    {
        var query = new GetCategoryById.Query { Id = id };
        var result = await mediator.Send(query);
        return result.IsSuccess ? Results.Ok(result.Value) : result.MapError();
    }

    // private static async Task<IResult> GetCategories(
    //     IMediator mediator,
    //     [FromQuery] int pageSize,
    //     [FromQuery] int pageNumber)
    // {
    //     var result = await mediator.Send(new GetCategories.Query { PageSize = pageSize, PageNumber = pageNumber });
    //
    //     return result.IsSuccess ? Results.Ok(result.Value) : result.MapError();
    // }

    private static async Task<IResult> UpdateCategory(IMediator mediator, [FromBody] UpdateCategory.Command command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }

    private static async Task<IResult> DeleteCategory(IMediator mediator, [FromRoute] int id)
    {
        var command = new DeleteCategory.Command { Id = id };
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }

    private static async Task<IResult> AddProductToCategory(
        IMediator mediator,
        [FromRoute] int categoryId,
        [FromRoute] int productId)
    {
        var command = new AddProductToCategory.Command { CategoryId = categoryId, ProductId = productId };
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }

    private static async Task<IResult> RemoveProductFromCategory(
        IMediator mediator,
        [FromRoute] int categoryId,
        [FromRoute] int productId)
    {
        var command = new RemoveProductFromCategory.Command { CategoryId = categoryId, ProductId = productId };
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }
}