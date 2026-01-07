
var customer = new Customer("", "", -15);

var notification = CustomerValidation.Validate(customer);

if (notification.HasErrors)
{
    notification.Errors.ToList().ForEach(error => Console.WriteLine($" - {error}"));
}
else
{
    Console.WriteLine("Cliente válido.");
}

    public record Customer(string Name, string Country, decimal Balance);

public static class CustomerValidation
{
    public static Notification Validate(Customer customer)
    {
        var notification = new Notification();

        if(string.IsNullOrWhiteSpace(customer.Name))
        {
            notification.add("Nombre es obligatorio.");
        }

        if(string.IsNullOrWhiteSpace(customer.Country))
        {
            notification.add("País es obligatorio.");
        }

        if(customer.Balance < 0)
        {
            notification.add("Balance no puede ser negativo.");
        }

        return notification;
    }
}

// Notification Pattern

public class Notification
{
    private readonly List<string> _errors = new();
    public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

    public bool HasErrors => _errors.Any();

    public void add(string error)
    {
        _errors.Add(error);
    }
}