using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;
using Weather.Api;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class EndpointGroupExtensions
{
    public static void AddEndpoints(this IServiceCollection services) =>
        AddEndpoints(services, Assembly.GetCallingAssembly());

    public static void AddEndpoints(this IServiceCollection services, params Assembly[] assemblies)
    {
        List<IEndpointGroup> endpoints = new();

        foreach (Assembly assembly in assemblies)
        {
            endpoints.AddRange(
                assembly.GetTypes()
                    .Where(x => typeof(IEndpointGroup).IsAssignableFrom(x) &&
                                x is { IsInterface: false, IsAbstract: false })
                    .Select(Activator.CreateInstance)
                    .Cast<IEndpointGroup>()
                    .ToList()
            );
        }

        foreach (IEndpointGroup endpoint in endpoints)
        {
            endpoint.RegisterServices(services);
        }

        services.AddSingleton(endpoints as IReadOnlyCollection<IEndpointGroup>);
    }

    public static void UseEndpoints(this WebApplication app)
    {
        ILogger<Program> logger = app.Services.GetService<ILogger<Program>>() ?? NullLogger<Program>.Instance;
        logger.LogDebug("Registering route groups");

        IReadOnlyCollection<IEndpointGroup> endpoints = app.Services.GetService<IReadOnlyCollection<IEndpointGroup>>()
                                                        ?? Array.Empty<IEndpointGroup>();

        foreach (IEndpointGroup endpoint in endpoints)
        {
            Type type = endpoint.GetType();
            logger.LogDebug("Registering routes from {Type}", type);
            endpoint.AddRoutes(app);
        }
    }
}