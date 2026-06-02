


using Polly;
using System.Text.Json;

var fileName = "post.json";
var url = " https://jsonplaceholder.typicode.com/posts";

var fallbackPolicy = Policy<List<Post>>
    .Handle<HttpRequestException>()
    .FallbackAsync(
        fallbackValue: await GetInfoByFile(fileName),
        onFallbackAsync: async (exception, context) =>
        {
            Console.WriteLine("Fallback: servicio no responde");
            Console.WriteLine($"Motivo: {exception.Exception.Message}");
            Console.WriteLine("Se utilizara la informacion de respaldo");
            await Task.CompletedTask;
        });

//Combinando politicas(patrones de resiliencia)

var retryPolicy = Policy<List<Post>>
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(2),
        onRetry: (outcome, timespan, retryCount, context) =>
        {
            Console.WriteLine($"Reintento {retryCount} en {timespan.TotalSeconds} segundos");
            Console.WriteLine($"Motivo {outcome.Exception?.Message}");
        });

//La que se ponga de ultima se ejecuta primero
var combinedPolicies = Policy.WrapAsync(fallbackPolicy, retryPolicy);


var posts = await combinedPolicies.ExecuteAsync(async () =>
{
   var httpClient = new HttpClient();
    Console.WriteLine("Solicitud al servicio");

    var response = await httpClient.GetAsync(url);

    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();

    var posts = JsonSerializer.Deserialize<List<Post>>(content, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });


    Console.WriteLine("Solicitud al servicio con exito");

    //guardar la info
    var fileContent = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true});
    await File.WriteAllTextAsync(fileName, fileContent);


    return posts;
});

posts.ForEach(post =>
{
    Console.WriteLine($"Post: {post.Id}, Title: {post.Title}");
});


async Task<List<Post>> GetInfoByFile(string fileName)
{
    if(!File.Exists(fileName))
        return new List<Post>();

    var fileContent = await File.ReadAllTextAsync(fileName);
    var posts = JsonSerializer.Deserialize<List<Post>>(fileContent);
    return posts;
}




record Post(int UserId, int Id, string Title, string Body);