using Polly;
using Polly.Timeout;

var timeoutPolicy = Policy.TimeoutAsync(
        timeout: TimeSpan.FromSeconds(4),
        timeoutStrategy: TimeoutStrategy.Optimistic,
        onTimeoutAsync: (context, timespan, task, exception) =>
        {
            Console.WriteLine($"Mucho tiempo en la operacion {timespan.TotalSeconds} segundos.");
            return Task.CompletedTask;
        }
    );


try
{
    await timeoutPolicy.ExecuteAsync(async (token) =>
    {
        // await Operation(TimeSpan.FromSeconds(10), token);
        await Execute(token);
    }, CancellationToken.None);

}catch(TimeoutRejectedException)
{
    Console.WriteLine("Excepcion arrojada por Polly por exceder el tiempo.");
}
catch (Exception ex)
{
    Console.WriteLine($"Otra excepcion: {ex.GetType().Name} - {ex.Message}");
}

Console.WriteLine("Observando durante 5s para ver si Execute() continua...");
await Task.Delay(5000);

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