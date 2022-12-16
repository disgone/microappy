using Serilog;
using Serilog.Exceptions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up");
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Logging
    builder.Host.UseSerilog((_, services, loggerConfig) =>
    {
        loggerConfig
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console();
    });

    // Configure Open API
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Compression/Caching
    builder.Services.AddOutputCache();
    
    builder.Services.AddEndpoints();
    
    var app = builder.Build();

    app.UseSerilogRequestLogging(opt =>
    {
        opt.EnrichDiagnosticContext = (dc, http) =>
        {
            dc.Set("RequestHost", http.Request.Host.Value);
            dc.Set("RequestScheme", http.Request.Scheme);
        };
    });
    
    app.UseOutputCache();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.Map("/", () => Results.Redirect("/swagger"));
    app.UseEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

return 0;

internal partial class Program
{
}