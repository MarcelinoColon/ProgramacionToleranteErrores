using ApiwithLogs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

/*
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "dd/MM/yyyy hh:mm:ss";
});
*/

//Con serilog se puede escribir el log en varios lugares como consola, archivos, etc, y tener un template para cada uno
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose() //Verbose es lo mismo que Trace en serilog
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console(
    //el Level w3 es para mostrar desde el level con 3 letras en minuscula
        outputTemplate: "{Timestamp:dd/MM/yyy HH:mm:ss} [{Level:w3}] {Message:lj} {RequestId}{NewLine}"
        )
    .WriteTo.File(
    path: "Logs/log.log",
    rollingInterval: RollingInterval.Day,
    retainedFileCountLimit: 10,
    //el Level u3 es para mostrar desde el level con 3 letras en mayusculas
    outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj} {Properties}{NewLine}"
    ).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/hi", (ILogger<Program> log) =>
{
    log.LogTrace("Se rastrea que entre al endpoint /hi");
    log.LogInformation("Entraron al endpoint /hi");
    log.LogWarning("!Cuidado! Han entrado al endpoint /hi");


    return "Hola Mundo";
});

app.MapPost("/checktotal", (Order order, ILogger<Program> log) =>
{
    try
    {
        log.LogInformation("Calculando total de la orden con {itemCount} items", order.detail.Count);

        decimal total = 0;

        if(ProductRepository.products.Count <= 0)
        {
            log.LogWarning("No hay productos en la base de datos");
        }

        foreach(var item in order.detail)
        {
            log.LogDebug("Buscando producto {productId} con la cantidad {quantity}", item.idProduct, item.Quantity);

            var product = ProductRepository.products.FirstOrDefault(p => p.Id == item.idProduct);

            if(product == null)
            {
                log.LogError("Producto {productId} no encontrado", item.idProduct);
                return Results.BadRequest($"Producto {item.idProduct} no encontrado");
            }

            log.LogDebug("Producto encontrado: {productName} - Precio: {price}", product.Name, product.Price);

            total += product.Price * item.Quantity;
        }


        log.LogDebug("Total de la orden calculado: {total}", total);
        return Results.Ok(total);
    }
    catch (Exception ex)
    {
        log.LogError(ex, "Error al calcular el total de la orden");
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.Run();

public record Order(List<OrderDetail> detail);
public record OrderDetail(int idProduct, int Quantity);
public record Product(int Id, string Name, decimal Price);