using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("MetalCodeApi", client =>
{
    client.BaseAddress = new Uri("https://metalcode2.io");
}).AddPolicyHandler(GetRetryPolicy());


//Manera mas simple de agregar politicas de polly
builder.Services.AddHttpClient("MetalCodeApiSimpleMode", client =>
{
    client.BaseAddress = new Uri("https://metalcode2.io");
}).AddTransientHttpErrorPolicy(policyBuilder =>
{
    return policyBuilder.WaitAndRetryAsync(5, attempt => TimeSpan.FromSeconds(2), 
        onRetry: (response, time, attempt, context) =>
    {
        Console.WriteLine($"Intento numero {attempt}");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/checkstatus", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("MetalCodeApi");

    try
    {
        Console.WriteLine("Haciendo solicitud a API externa");

        var response = await client.GetAsync("status");

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"API externa fallo con codigo {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();

        return Results.Ok($"Respuesta externa: {content}");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al llamar a API externa: {ex.Message}");
    }
});

app.MapGet("/checkstatussimplemode", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("MetalCodeApiSimpleMode");

    try
    {
        Console.WriteLine("Haciendo solicitud a API externa");

        var response = await client.GetAsync("status");

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"API externa fallo con codigo {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();

        return Results.Ok($"Respuesta externa: {content}");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al llamar a API externa: {ex.Message}");
    }
});

app.Run();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return Policy.Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(3),
        onRetry: (result, time, attempt, context) =>
        {
            if (result.Exception != null)
            {
                Console.WriteLine($"Excepcion: {result.Exception.Message}");
            }
            else
            {
                Console.WriteLine($"Codigo HTTP fallido: {result.Result.StatusCode}");
            }

            Console.WriteLine($"Reintento {attempt} despues de {time.TotalNanoseconds}s...\n");
        }
        );
}
