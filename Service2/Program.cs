using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/", () => "Hello from backend 5002!");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Open API V1");
    });

    app.UseReDoc(options =>
    {
        options.SpecUrl("/openapi/v1.json");
    });

    app.MapScalarApiReference();
}

app.Run("http://localhost:5002");

