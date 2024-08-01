using Application.Features.Commands.CreateCategory;
using Application.Features.Commands.UpdateCategory;
using Application.Features.Commands.DeleteCategory;
using Application.Features.Queries.GetCategoryById;
using MediatR;
using Presentation.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
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
                .Produces<GetCategoryById.CategoryDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/{id}", UpdateCategory)
                .WithName("UpdateCategory")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status400BadRequest);

            group.MapDelete("/{id}", DeleteCategory)
                .WithName("DeleteCategory")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);
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

        private static async Task<IResult> UpdateCategory(IMediator mediator, [FromRoute] int id, [FromBody] UpdateCategory.Command command)
        {
            if (id != command.Id)
                return Results.BadRequest("ID in route does not match ID in body");

            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.NoContent() : result.MapError();
        }

        private static async Task<IResult> DeleteCategory(IMediator mediator, [FromRoute] int id)
        {
            var command = new DeleteCategory.Command { Id = id };
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.NoContent() : result.MapError();
        }
    }
}