using Application.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Extensions;
using System.Net.Http.Json;

namespace Presentation.Api.IntegrationTests.Extensions;

public static class ProblemDetailsExtensions
{
    public static Task<ProblemDetails> ReadAndAssertProblemDetailsAsync(this HttpResponseMessage response, HttpStatusCode expectedStatusCode, string expectedDetail, ErrorType errorType)
    {
        var error = Error.Create(errorType, expectedDetail);

        return response.ReadAndAssertProblemDetailsAsync(expectedStatusCode, error);
    }

    public static async Task<ProblemDetails> ReadAndAssertProblemDetailsAsync(this HttpResponseMessage response, HttpStatusCode expectedStatusCode, Error error)
    {
        response.StatusCode.Should().Be(expectedStatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ShouldHaveProblemDetails(expectedStatusCode, error.Description, error.Type);

        return problemDetails!;
    }

    private static void ShouldHaveProblemDetails(this ProblemDetails problemDetails, HttpStatusCode expectedStatusCode, string expectedDetail, ErrorType errorType)
    {
        problemDetails.Status.Should().Be((int)expectedStatusCode);
        problemDetails.Title.Should().Be(expectedStatusCode.ToString().ToPascalCase());
        problemDetails.Detail.Should().Contain(expectedDetail);
        problemDetails.Extensions.Should().ContainKey("errorCode");

        var errorCode = problemDetails.Extensions["errorCode"];
        errorCode.Should().NotBeNull();
        errorCode!.ToString().Should().Be(errorType.ToString());
    }
}