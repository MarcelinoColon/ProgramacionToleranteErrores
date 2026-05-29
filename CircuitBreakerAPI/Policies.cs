using Polly;

namespace CircuitBreakerAPI
{
    public static class Policies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy<HttpResponseMessage>.HandleResult(r => !r.IsSuccessStatusCode)
                                              .Or<HttpRequestException>()
                                              .CircuitBreakerAsync(
                                               handledEventsAllowedBeforeBreaking: 3,
                                               durationOfBreak: TimeSpan.FromSeconds(10),
                                               onBreak: (outcome, timespan) =>
                                               {
                                                   Console.WriteLine($"Circuito ABIERTO por {timespan.TotalSeconds}s. Motivo: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                                               },
                                               onReset: () => Console.WriteLine("Circuito CERRADO: Conexion estable"),
                                               onHalfOpen: () => Console.WriteLine("HALF-OPEN: probando nuevamente")
                                               );
        }

        // El Advanced Circuit Breaker es una version mas sofisticada que tiene en cuenta el porcentaje de fallos, el volumen de trafico
        // y el tiempo de muestreo para decidir cuando abrir o cerrar el circuito.
        public static IAsyncPolicy<HttpResponseMessage> GetAdvancedCircuitBreakerPolicy()
        {
            return Policy<HttpResponseMessage>.HandleResult(r => !r.IsSuccessStatusCode)
                                              .Or<HttpRequestException>()
                                              .AdvancedCircuitBreakerAsync(
                                               failureThreshold: 0.5,
                                               samplingDuration: TimeSpan.FromSeconds(20),
                                               minimumThroughput: 6,
                                               durationOfBreak: TimeSpan.FromSeconds(10),
                                               onBreak: (outcome, timespan) =>
                                               {
                                                   Console.WriteLine($"Circuito ABIERTO por {timespan.TotalSeconds}s. Motivo: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                                               },
                                               onReset: () => Console.WriteLine("Circuito CERRADO: Conexion estable"),
                                               onHalfOpen: () => Console.WriteLine("HALF-OPEN: probando nuevamente")
                                               );
        }
    }
}
