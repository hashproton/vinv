using Microsoft.OpenApi.Models;
using Application.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Api.Extensions.DependencyInjection;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

            c.CustomSchemaIds(GetCustomSchemaId);

            c.MapType<Result>(CreateResultSchema);

            // Use SchemaFilter to handle Result<T>
            c.SchemaFilter<ResultSchemaFilter>();

            // Add a custom operation filter to handle ProducesValidationProblem
            c.OperationFilter<ValidationProblemOperationFilter>();
        });

        return services;
    }

    private static string GetCustomSchemaId(Type type)
    {
        if (type.IsNested)
        {
            return $"{type.DeclaringType?.Name}.{type.Name}";
        }
        
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericArg = type.GetGenericArguments().FirstOrDefault();
            return genericArg != null ? $"Result<{GetCustomSchemaId(genericArg)}>" : "Result";
        }

        return type.Name;
    }

    private static OpenApiSchema CreateResultSchema()
    {
        return new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                { "isSuccess", new OpenApiSchema { Type = "boolean" } },
                { "error", new OpenApiSchema 
                    {
                        Type = "object",
                        Nullable = true,
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            { "code", new OpenApiSchema { Type = "string" } },
                            { "message", new OpenApiSchema { Type = "string" } }
                        }
                    }
                }
            }
        };
    }
}

public class ResultSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsGenericType && context.Type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = context.Type.GetGenericArguments()[0];
            var valueSchema = context.SchemaGenerator.GenerateSchema(valueType, context.SchemaRepository);

            schema.Type = "object";
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                { "isSuccess", new OpenApiSchema { Type = "boolean" } },
                { "value", valueSchema },
                { "error", new OpenApiSchema 
                    {
                        Type = "object",
                        Nullable = true,
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            { "code", new OpenApiSchema { Type = "string" } },
                            { "message", new OpenApiSchema { Type = "string" } }
                        }
                    }
                }
            };
        }
    }
}

public class ValidationProblemOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var producesValidationProblem = context.ApiDescription.CustomAttributes()
            .OfType<ProducesResponseTypeAttribute>()
            .Any(x => x.Type == typeof(ValidationProblem));

        if (producesValidationProblem)
        {
            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Validation error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "ValidationProblemDetails" }
                        }
                    }
                }
            });
        }
    }
}