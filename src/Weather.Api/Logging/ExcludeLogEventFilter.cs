using Serilog.Core;
using Serilog.Events;

namespace Weather.Api.Logging;

public class ExcludeLogEventFilter : ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent) =>
        !logEvent.Properties.TryGetValue("RequestPath", out LogEventPropertyValue? requestPath) ||
        requestPath is not ScalarValue
        {
            Value: "/metrics"
        };
}