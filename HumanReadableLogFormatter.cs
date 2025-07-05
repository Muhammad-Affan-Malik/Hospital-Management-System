using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;

public class HumanReadableLogFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var date = logEvent.Timestamp.ToString("yyyy-MM-dd");
        var level = logEvent.Level.ToString().ToUpper();
        var message = logEvent.RenderMessage();

        output.WriteLine($"[{date}] {level}: {message}");
    }
}
