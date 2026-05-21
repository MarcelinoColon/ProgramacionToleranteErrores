
var retryOperation = new Retry<string>(async () =>
{
    var http = new HttpClient();

    var response = await http.GetAsync("https://somesite.io");

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"Operacion fallida con codigo de estado {response.StatusCode}.");
    }

    return await response.Content.ReadAsStringAsync();

}, 5, 2000);


try
{
    string result = await retryOperation.Execute();
    Console.WriteLine($"Operacion exitosa con resultado: {result.Substring(0, 50)}...");
}catch(Exception ex)
{
    Console.WriteLine($"Operacion fallida despues de varios intentos: {ex.Message}");
}


class Retry<T>
{
    private readonly Func<Task<T>> _operation;
    private readonly int _maxRetries;
    private readonly int _delayMilliseconds;

    public Retry(Func<Task<T>> operation, int maxRetries = 3, int delayMilliseconds = 1000)
    {
        _operation = operation;
        _maxRetries = maxRetries;
        _delayMilliseconds = delayMilliseconds;
    }

    public async Task<T> Execute()
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            try
            {
                return await _operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Intento {attempt} ha fallado: {ex.Message}");
                await Task.Delay(_delayMilliseconds);
            }
        }

        throw new Exception("Todos los reintentos han fallado.");
    }
}