using System.Net.Mime;
using Application.Features.Commands.CreateTenant;
using Application.Features.Commands.DeleteTenant;
using Application.Features.Commands.UpdateProduct;
using Application.Features.Commands.UpdateTenant;
using Application.Features.Queries.GetTenantById;
using Application.Features.Queries.GetTenantsPaged;
using Application.Repositories.Shared;
using Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Api.Extensions;

namespace Presentation.Api.Endpoints;

public static class TenantEndpoints
{
    public static IEndpointRouteBuilder MapTenantsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tenants").WithTags("Tenants");

        group.MapPost("/", CreateTenant)
            .WithName("CreateTenant")
            .WithSummary("Creates a new tenant.")
            .WithDescription("Creates a new tenant.")
            .Accepts<CreateTenant.Command>(MediaTypeNames.Application.Json)
            .Produces<Result<int>>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapGet("/{id:int}", GetTenantById)
            .WithName("GetTenantById")
            .WithSummary("Gets a tenant by ID.")
            .WithDescription("Retrieves a tenant by its unique ID.")
            .Produces<Result<GetTenantById.TenantDto>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/", GetTenantsPaged)
            .WithName("GetTenantsPaged")
            .WithSummary("Gets all tenants.")
            .WithDescription("Retrieves all tenants in the system.")
            .Produces<Result<PagedResult<GetTenantsPaged.TenantDto>>>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/", UpdateTenant)
            .WithName("UpdateTenant")
            .WithSummary("Updates an existing tenant.")
            .WithDescription("Updates an existing tenant details.")
            .Accepts<UpdateProduct.Command>(MediaTypeNames.Application.Json)
            .Produces<Result>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteTenant)
            .WithName("DeleteTenant")
            .WithSummary("Deletes a tenant by ID.")
            .WithDescription("Deletes a tenant from the database using its unique ID.")
            .Produces<Result>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<IResult> CreateTenant(IMediator mediator, [FromBody] CreateTenant.Command command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.Created($"/api/tenants/{result.Value}", result.Value)
            : result.MapError();
    }

    private static async Task<IResult> GetTenantsPaged(
        IMediator mediator,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        [FromQuery] string? filter = null)
    {
        var query = new GetTenantsPaged.Query
        {
            PagedQuery = new PagedQuery
            {
                Skip = skip,
                Take = take,
                Filter = filter
            }
        };

        var result = await mediator.Send(query);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.MapError();
    }

    private static async Task<IResult> GetTenantById(IMediator mediator, [FromRoute] int id)
    {
        var query = new GetTenantById.Query { Id = id };
        var result = await mediator.Send(query);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.MapError();
    }

    private static async Task<IResult> UpdateTenant(IMediator mediator, [FromBody] UpdateTenant.Command command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.NoContent()
            : result.MapError();
    }

    private static async Task<IResult> DeleteTenant(IMediator mediator, [FromRoute] int id)
    {
        var command = new DeleteTenant.Command { Id = id };
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.NoContent()
            : result.MapError();
    }
}