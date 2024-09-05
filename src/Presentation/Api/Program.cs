using System.Runtime.CompilerServices;
using Infra;
using Presentation.Api;
using Presentation.Api.Endpoints;
using Presentation.Api.Extensions.DependencyInjection;
using Presentation.Api.Middlewares;

[assembly: InternalsVisibleTo("tests.Presentation.Api.IntegrationTests")]

var builder = WebApplication.CreateBuilder(args);

var isDevelopmentOrTest = builder.Environment.IsDevelopment() || builder.Environment.IsTest();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: isDevelopmentOrTest, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: !isDevelopmentOrTest,
        reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(Program).Assembly, optional: true)
    .AddCommandLine(args);

builder.Services
    .AddInfra(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCors();

var app = builder.Build();

app.UseExceptionHandler(_ =>
{
});

if (app.Environment.IsDevelopment())
{
    app.UseCors(b => b
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
    .MapTenantsEndpoints()
    .MapCategoryEndpoints()
    .MapProductEndpoints();

await app.RunAsync();