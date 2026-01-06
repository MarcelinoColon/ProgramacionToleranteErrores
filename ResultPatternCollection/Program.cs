
var results = new List<Result<decimal>>();
int n = 100;

for(int i = 0; i < n; i++)
{
    decimal number = Random.Shared.Next(-100, 100);
    Result<decimal> result;

    if(number > 0)
    {
        result = Result<decimal>.Success(number);
    }
    else
    {
        result = Result<decimal>.Failure("El numero no es positivo");
    }

    results.Add(result);
}

results.ForEach(result =>
{
    if(result.IsSuccess)
    {
        Console.WriteLine($"Numero positivo: {result.Value}");
    }
    else
    {
        Console.WriteLine($"Error: {result.Error}");
    }
});





//Result Pattern
public class Result<T>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public T? Value { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }
    private Result(string error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}