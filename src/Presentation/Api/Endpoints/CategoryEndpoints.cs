using Application.Features.Commands.AddProductToCategory;
using Application.Features.Commands.CreateCategory;
using Application.Features.Commands.DeleteCategory;
using Application.Features.Commands.RemoveProductFromCategory;
using Application.Features.Commands.UpdateCategory;
using Application.Features.Queries.GetCategoryById;
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
            .Produces<int>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id}", GetCategoryById)
            .WithName("GetCategoryById")
            .Produces<GetCategoryById.CategoryDto>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/", UpdateCategory)
            .WithName("UpdateCategory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", DeleteCategory)
            .WithName("DeleteCategory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        
        group.MapPost("/{categoryId}/products/{productId}", AddProductToCategory)
            .WithName("AddProductToCategory")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
        
        group.MapDelete("/{categoryId}/products/{productId}", RemoveProductFromCategory)
            .WithName("RemoveProductFromCategory")
            .Produces(StatusCodes.Status204NoContent)
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
    
    private static async Task<IResult> AddProductToCategory(IMediator mediator, [FromRoute] int categoryId, [FromRoute] int productId)
    {
        var command = new AddProductToCategory.Command { CategoryId = categoryId, ProductId = productId };
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }
    
    private static async Task<IResult> RemoveProductFromCategory(IMediator mediator, [FromRoute] int categoryId, [FromRoute] int productId)
    {
        var command = new RemoveProductFromCategory.Command { CategoryId = categoryId, ProductId = productId };
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }
}