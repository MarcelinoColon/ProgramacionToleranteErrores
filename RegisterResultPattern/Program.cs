using System.Text.RegularExpressions;

var service = new UserService();

Console.WriteLine("Escribe un correo: ");
var email = Console.ReadLine();

Console.WriteLine("Escribe una contraseña: ");
var password = Console.ReadLine();

Console.WriteLine("Confimr la contraseña: ");
var confirmPassword = Console.ReadLine();

var registerResult = service.Register(email, password, confirmPassword);

if (registerResult.IsSuccess)
{
    Console.WriteLine($"Usuario registrado con exito. Id: {registerResult.Value?.Id}");
}
else
{
    Console.WriteLine(registerResult.Error);
}


public class UserService
{
    private readonly List<User> _users = new();

    public Result<User> Register(string email, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Result<User>.Failure("El correo y la contraseña son obligatorios");
        }

        if (!Regex.IsMatch(email, @"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$"))
        {
            return Result<User>.Failure($"El correo [{email}] no tiene un formato valido");
        }

        if(_users.Any(u => u.Email.Equals(email)))
        {
            return Result<User>.Failure($"El correo [{email}] ya esta registrado");
        }

        if(password != confirmPassword)
        {
            return Result<User>.Failure("Las contraseñas no coinciden");
        }

        var user = new User(Guid.NewGuid(), email, password);
        _users.Add(user);

        return Result<User>.Success(user);
    }
}


public record User(Guid Id, string Email, string Password);


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