using Polly;
using Polly.Timeout;

namespace TimeoutAPI
{
    public class Policies
    {
        public IAsyncPolicy TimeoutPolicy(int seconds)
        {
            return Policy.TimeoutAsync(
                TimeSpan.FromSeconds(seconds),
                TimeoutStrategy.Optimistic,
                onTimeoutAsync: (context, timespan, task, exception) =>
                {
                    Console.WriteLine($"Se ha excedido el tiempo: {timespan.TotalSeconds} segundos.");
                    return Task.CompletedTask;
                }
             );
        }
    }
}
