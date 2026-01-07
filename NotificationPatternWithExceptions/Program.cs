
var climateGetters = new List<GetClimate>
{
    new GetClimate("19.43", "-99.13"), // Mexico City
    new GetClimate("34.05", "-118.25"), // Los Angeles
    new GetClimate("51.51", "-0.13"), // London
    new GetClimate("35.68", "139.69"), // Tokyo
    new GetClimate("-33.87", "151.21") // Sydney
};

var getterInfo = new GetterInfo(climateGetters);
var notification = await getterInfo.ExecuteAllAsync();

if(notification.HasErrors)
{
    Console.WriteLine("Ocurrieron algunos errores:");
    foreach (var error in notification.Errors)
    {
        Console.WriteLine($"- {error}");
    }
}
else
{
    Console.WriteLine("Todas las solicitudes fueron exitosas");
}

public class GetterInfo
{
    private readonly List<GetClimate> _climateGetters = new();

    public GetterInfo(List<GetClimate> climateGetters)
        => _climateGetters = climateGetters;

    public async Task<Notification> ExecuteAllAsync()
    {
        var notification = new Notification();

        foreach(var getter in _climateGetters)
        {
           var notificationResult = await getter.ExecuteAsync();

            if(notificationResult.HasErrors)
            {
                foreach(var error in notificationResult.Errors)
                {
                    notification.add(error);
                }
            }
        }

        return notification;
    }
}

public class GetClimate
{
    private readonly HttpClient _http = new();
    private const string ApiUrl = "https://api.open-meteo.com/v1/forecast?current_weather=true&latitude=19.43&longitude=-99.13";
    private string _lat;
    private string _long;

    public GetClimate(string lat, string lon)
    {
        _lat = lat;
        _long = lon;
    }

    public async Task<Notification> ExecuteAsync()
    {
        var notification = new Notification();

        try
        {
            var resp = await _http.GetAsync($"{ApiUrl}&latitude={_lat}&longitude={_long}");
            if (!resp.IsSuccessStatusCode)
            {
                notification.add($"Error en API ({_lat}, {_long}): {resp.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            notification.add($"Error: {ex.Message}");
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
