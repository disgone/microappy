using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        var resources = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName);

        builder.Services.AddOpenTelemetryMetrics(metrics =>
        {
            metrics.SetResourceBuilder(resources)
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEventCountersInstrumentation()
                .AddProcessInstrumentation()
                .AddPrometheusExporter();
        });

        builder.Services.AddOpenTelemetryTracing(tracing =>
        {
            tracing.SetResourceBuilder(resources)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
        });

        return builder;
    }
}