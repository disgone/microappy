using Serilog.Core;
using Serilog.Events;

namespace Weather.Api.Logging;

public class ExcludeLogEventFilter: ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent)
    {
        return !logEvent.Properties.TryGetValue("RequestPath", out var requestPath) || requestPath is not ScalarValue
        {
            Value: "/metrics"
        };
    }
}