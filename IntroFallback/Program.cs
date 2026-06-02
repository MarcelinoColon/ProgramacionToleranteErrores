using Polly;

var fallbackPolicy = Policy<decimal>.Handle<HttpRequestException>()
                            .FallbackAsync(
                               fallbackValue: 18.03m,
                                onFallbackAsync: (exception, context) =>
                                {
                                    Console.WriteLine("Fallback: Servicio no responde");
                                    Console.WriteLine($"Motivo {exception.Exception.Message}");
                                    return Task.CompletedTask;
                                }
                            );

decimal dollarRate = await fallbackPolicy.ExecuteAsync(async () =>
{
    return await GetDollarRate();
});

Console.WriteLine($"la tasa del dolar es: {dollarRate}");

async Task<decimal> GetDollarRate()
{
    Console.WriteLine("Solitud a servicio");
    throw new HttpRequestException();
}