using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Presentation.Api.Middlewares;

public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private readonly IHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));
    private readonly ILogger<GlobalExceptionHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = CreateProblemDetails(httpContext, exception);
        var json = ToJson(problemDetails);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsync(json, cancellationToken);

        _logger.LogError(exception, "An unhandled exception occurred.");

        return true;
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        var statusCode = context.Response.StatusCode != StatusCodes.Status200OK
            ? context.Response.StatusCode
            : StatusCodes.Status500InternalServerError;

        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode) ?? UnhandledExceptionMsg;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = reasonPhrase,
            Type = "InternalServerError",
            Extensions = {
                ["errorCode"] = exception.GetType().Name
            }
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Detail = exception.ToString();
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            problemDetails.Extensions["data"] = exception.Data;
        }

        return problemDetails;
    }

    private string ToJson(ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails, SerializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception has occurred while serializing error to JSON");
            var fallbackProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = "An error occurred while serializing the problem details."
            };

            return JsonSerializer.Serialize(fallbackProblemDetails, SerializerOptions);
        }
    }
}