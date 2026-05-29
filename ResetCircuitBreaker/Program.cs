using Polly;
using Polly.CircuitBreaker;

var urlError = "https://tools-httpstatus.pickup-services.com/500";

var urlSuccess = "https://tools-httpstatus.pickup-services.com/200";

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
                                      Console.WriteLine("Circuito CLOSED: el servicio volvio a funcionar.");
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
            var url = i > 5 ? urlSuccess : urlError;

            var response = await httpClient.GetAsync(url);
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