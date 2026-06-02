using Polly;
using Polly.Timeout;

var timeoutPolicy = Policy.TimeoutAsync(
        timeout: TimeSpan.FromSeconds(20),
        timeoutStrategy: TimeoutStrategy.Optimistic,
        onTimeoutAsync: (context, timespan, task, exception) =>
        {
            Console.WriteLine($"Mucho tiempo en la operacion {timespan.TotalSeconds} segundos.");
            return Task.CompletedTask;
        }
    );


using var ctSource = new CancellationTokenSource();

var task = Task.Run(async () =>
{
    try
    {
        await timeoutPolicy.ExecuteAsync(async (token) =>
        {
            // await Operation(TimeSpan.FromSeconds(10), token);
            await Execute(token);
        }, ctSource.Token);

    }
    catch (TimeoutRejectedException) //Excepcion lanzada por Polly cuando se excede el tiempo establecido en la politica de timeout
    {
        Console.WriteLine("Excepcion arrojada por Polly por exceder el tiempo.");
    }
    catch (OperationCanceledException) // Excepcion lanzada cuando se cancela la operacion manualmente, ya sea por el token de cancelacion o por el timeout de Polly en la estrategia optimista
    {
        Console.WriteLine("Operacion cancelada manualmente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Otra excepcion: {ex.GetType().Name} - {ex.Message}");
    }
});

await Task.Delay(5000);
Console.WriteLine("Se cancela manualmente");
ctSource.Cancel();

await task;

async Task Operation(TimeSpan delay, CancellationToken ct)
{
    Console.WriteLine("Haciendo algo...");
    await Task.Delay(delay, ct);
}

async Task Execute(CancellationToken ct)
{
    while (true)
    {
        Console.WriteLine("Haciendo algo");
        await Task.Delay(1000, ct);
    }
}