using Polly;

var httpClient = new HttpClient();

var retryPolicy = Policy.Handle<HttpRequestException>()
    .WaitAndRetryAsync(
    retryCount: 3,
    //sleepDurationProvider: attempt => TimeSpan.FromSeconds(2),
    //sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 2),
    //sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Random.Shared.Next(3,8)),
    onRetry: (exception, timeSpan, retryCount, context) =>
    {
        Console.WriteLine($"[{context.OperationKey} de {context["age"]} años]Intento {retryCount} despues de {timeSpan.TotalSeconds} segundos: {exception.Message}");
    });


var context = new Context("Juanito");
context["age"] = 15;

try
{
    await retryPolicy.ExecuteAsync(async (context) =>
    {
        Console.WriteLine(context.OperationKey);
        var response = await httpClient.GetAsync("https://somesite.io");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Respuesta recibida con exito");
        Console.WriteLine(content);
    }, context
    //new Context("juanito")
    );

}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Operacion fallida despues de reintentos: {ex.Message}");
}