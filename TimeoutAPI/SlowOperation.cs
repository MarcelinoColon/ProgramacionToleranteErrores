namespace TimeoutAPI
{
    public class SlowOperation
    {
        public async Task Execute(CancellationToken ct)
        {
            Console.WriteLine("Operacion iniciada");

            await Task.Delay(TimeSpan.FromSeconds(5), ct);

            Console.WriteLine("Operacion finalizada");
        }
    }
}
