using Infra;
using Presentation.Api;
using Presentation.Api.Endpoints;
using Presentation.Api.Middlewares;

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
    .AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(type =>
        {
            var fullName = type.FullName;
            if (type.IsNested)
            {
                return type.DeclaringType?.Name;
            }

            return fullName;
        });
    })
    .AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler(_ =>
{
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
    .MapCategoryEndpoints()
    .MapProductEndpoints();

app.Run();

public partial class Program;