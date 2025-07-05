using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

public static class LoggerConfig
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
     .MinimumLevel.Debug()
     .Enrich.WithProperty("Application", "HospitalManagementSystem")
     .Enrich.FromLogContext()
     .WriteTo.Console()
     .WriteTo.File(
         new Serilog.Formatting.Json.JsonFormatter(),
         "Logs/log.json",
         rollingInterval: RollingInterval.Day)
     .CreateLogger();
    }
}
