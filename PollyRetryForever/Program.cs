using Polly;
using System.Net.NetworkInformation;

string host = "metalcode2.io";

var pingPolicy = Policy.Handle<PingException>()
    .OrResult<PingReply>(r => r.Status != IPStatus.Success)
    .WaitAndRetryForeverAsync(
    sleepDurationProvider: attemp => TimeSpan.FromSeconds(3),
    onRetry: (result, attempt, time) =>
    {
        string error = result.Exception != null ?
        result.Exception.Message :
        result.Result.Status.ToString();

        Console.WriteLine($"Reintento {attempt}. Error causado: {error}. Reintentado en {time.TotalSeconds} segundos. ");
    }
    );


var context = new Context("Ping");
context["Host"] = host;

await pingPolicy.ExecuteAsync(async (context) =>
{
    using var ping = new Ping();
    Console.WriteLine($"Enviando ping a [{context["Host"]}]");


    var reply = await ping.SendPingAsync((string)context["Host"], 2000);

    if(reply.Status == IPStatus.Success)
    {
        Console.WriteLine($"Respuesta recibida desde {reply.Address} en {reply.RoundtripTime}ms");
    }
    else
    {
        Console.WriteLine($"No hay respuesta {reply.Status}");
    }

    return reply;
}, context);