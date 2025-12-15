using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductRepository_Exception;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (status, title) = ex switch
        {
            ValidationException or ArgumentException or BadHttpRequestException
            => (StatusCodes.Status400BadRequest, "Error de validacion"),
            KeyNotFoundException
            => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            TimeoutException or TaskCanceledException or OperationCanceledException
            => (StatusCodes.Status504GatewayTimeout, "Tiempo de espera agotado"),
            _
            => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = ex?.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem);
    });
});

app.UseHttpsRedirection();

app.MapGet("/products/{id}", async (int id, ProductRepository repository) =>
{
    var product = repository.GetProductById(id);

    return Results.Ok(product);
});




app.Run();


