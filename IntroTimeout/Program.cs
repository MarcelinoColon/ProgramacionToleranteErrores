using Polly;
using Polly.Timeout;


// la estrategia pesimista no se asegura de que la tarea que se esta ejecutando se cancele, simplemente no espera a que termine y lanza la excepcion de timeout,
// mientras que la estrategia optimista si se asegura de cancelar la tarea, pero requiere que la tarea sea cancelable y que se le pase un token de cancelacion,
// ademas de que la tarea debe cooperar con el token de cancelacion para que funcione correctamente.
var timeoutPolly = Policy.TimeoutAsync(
         timeout: TimeSpan.FromSeconds(2),
         timeoutStrategy: TimeoutStrategy.Pessimistic,
         onTimeoutAsync: (context, timespan, task, exception) =>
         {
             Console.WriteLine($"[{context["Id"]}] Mucho tiempo en la operacion {timespan.TotalSeconds} segundos.");
             return Task.CompletedTask;
         }
         );

var context = new Context();
context["Id"] = "123123";

try
{
    await timeoutPolly.ExecuteAsync(async (context) =>
    {
        //await Operation(TimeSpan.FromSeconds(10));
        await Execute();
    }, context);
}
catch (TimeoutRejectedException)
{
    Console.WriteLine("Excepcion arrojada por Polly por exceder el tiempo.");
}
catch (Exception ex)
{
    Console.WriteLine($"Otra excepcion: {ex.GetType().Name} - {ex.Message}");
}

Console.WriteLine("Observando durante 5s para ver si se sigue ejecutando");
await Task.Delay(5000);

async Task Operation(TimeSpan delay)
{
    Console.WriteLine("Haciendo algo...");
    await Task.Delay(delay);
}

async Task Execute()
{
    while (true)
    {
        Console.WriteLine("Haciendo algo");
        await Task.Delay(1000);
    }
}