using Presentation.Api.Endpoints;
using Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfra(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCategoryEndpoints();

app.Run();

public partial class Program;
