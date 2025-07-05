using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.IO;

public static class LoggerConfig
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                new HumanReadableLogFormatter(),
                path: Path.Combine(AppContext.BaseDirectory, "logs", "Logs.txt"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true)
            .CreateLogger();
    }
}
