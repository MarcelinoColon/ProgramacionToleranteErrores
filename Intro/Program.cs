using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder =>
{
    //Nivel mas alto None para no mostrar ningun log, nivel minimo es trace
    builder
    .AddSimpleConsole(options =>
    {
        options.TimestampFormat = "dd/MM/yyyy hh:mm:ss";
        options.SingleLine = true;
    })
    .SetMinimumLevel(LogLevel.Trace);
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogTrace("Nivel trace");
logger.LogDebug("Nivel debug");
logger.LogInformation("Nivel informacion");
logger.LogWarning("Nivel advertencia");
logger.LogError("Nivel error");
logger.LogCritical("Nivel critico");

logger.LogInformation("Iniciando la aplicacion...");

int res = 5 + 10;

logger.LogInformation("El resultado es: {resultado}", res);

logger.LogInformation("Fin de la aplicacion");

loggerFactory.Dispose();
