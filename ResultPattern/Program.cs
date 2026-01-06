
var readNumber = new ReadNumber();

var numberResult = readNumber.Read();

if (numberResult.IsSuccess)
{
    Console.WriteLine($"El numero capturado es: {numberResult.Value}");
}
else
{
    Console.WriteLine(numberResult.Error);
}

public class ReadNumber
{
    public Result<int> Read()
    {
        Console.Write("Escribe un numero: ");
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int number))
        {
            var result = Result<int>.Failure($"El valor [{input}] no es un numero valido");
            return result;
        }
        return Result<int>.Success(number);
    }
}


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