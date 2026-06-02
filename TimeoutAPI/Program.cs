using Polly.Timeout;
using TimeoutAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<Policies>();
builder.Services.AddTransient<SlowOperation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/timeout", async (Policies policies, SlowOperation slowOperation) =>
{
    try
    {
        await policies.TimeoutPolicy(2).ExecuteAsync(async (ct) =>
        {
            await slowOperation.Execute(ct);
        }, CancellationToken.None);

        return Results.Ok("Operacion completada exitosamente.");
    }
    catch (TimeoutRejectedException)
    {
        return Results.Problem("La operacion ha excedido el tiempo permitido.");
    }
});


app.Run();


