﻿using System.Net.Mime;
using Application.Features.Commands.CreateProduct;
using Application.Features.Commands.DeleteProduct;
using Application.Features.Commands.UpdateProduct;
using Application.Features.Queries.GetProductById;
using Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Api.Extensions;

namespace Presentation.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapPost("/", CreateProduct)
            .WithName("CreateProduct")
            .WithSummary("Creates a new product.")
            .WithDescription("Creates a new product and assigns it to a category.")
            .Accepts<CreateProduct.Command>(MediaTypeNames.Application.Json)
            .Produces<Result<int>>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id:int}", GetProductById)
            .WithName("GetProductById")
            .WithSummary("Gets a product by ID.")
            .WithDescription("Retrieves a product by its unique ID.")
            .Produces<Result<GetProductById.ProductDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/", UpdateProduct)
            .WithName("UpdateProduct")
            .WithSummary("Updates an existing product.")
            .WithDescription("Updates an existing product's details including its category.")
            .Accepts<UpdateProduct.Command>(MediaTypeNames.Application.Json)
            .Produces<Result>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithSummary("Deletes a product by ID.")
            .WithDescription("Deletes a product from the database using its unique ID.")
            .Produces<Result>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        
        return app;
    }

    private static async Task<IResult> CreateProduct(IMediator mediator, [FromBody] CreateProduct.Command command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess
            ? Results.Created($"/api/products/{result.Value}", result.Value)
            : result.MapError();
    }

    private static async Task<IResult> GetProductById(IMediator mediator, [FromRoute] int id)
    {
        var query = new GetProductById.Query { Id = id };
        var result = await mediator.Send(query);
        return result.IsSuccess ? Results.Ok(result.Value) : result.MapError();
    }

    private static async Task<IResult> UpdateProduct(IMediator mediator, [FromBody] UpdateProduct.Command command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }

    private static async Task<IResult> DeleteProduct(IMediator mediator, [FromRoute] int id)
    {
        var command = new DeleteProduct.Command { Id = id };
        var result = await mediator.Send(command);
        return result.IsSuccess ? Results.NoContent() : result.MapError();
    }
}
