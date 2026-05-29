using Polly;
using Polly.CircuitBreaker;

var circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                                 .CircuitBreakerAsync(
                                  exceptionsAllowedBeforeBreaking: 3,
                                  durationOfBreak: TimeSpan.FromSeconds(5),
                                  onBreak: (ex, breakDealy) =>
                                  {
                                      Console.WriteLine($"Circuito OPEN: {ex.Message}." +
                                          $"No se intentara nuevamente por {breakDealy.TotalSeconds} segundos.");
                                  },
                                  onReset: () =>
                                  {
                                      Console.WriteLine("Circuito CLOSED: el servicio vovio a funcionar.");
                                  },
                                  onHalfOpen: () =>
                                  {
                                      Console.WriteLine("Circuito HALF-OPEN: probando nuevamente...");
                                  }
                                  );

using var httpClient = new HttpClient();

for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"Intento {i}");

    try
    {
        await circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            var response = await httpClient.GetAsync("https://metalcode2.io");
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Solicitud exitosa");
        });
    }
    catch (BrokenCircuitException)
    {
        Console.WriteLine("El circuito esta abierto, no se realizara la llamada");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    await Task.Delay(2000);
}