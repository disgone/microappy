namespace Weather.Api;

public interface IEndpointGroup
{
    /// <summary>
    /// Adds registrations to the provided <see cref="IServiceCollection"/> for this endpoint group.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
    public void RegisterServices(IServiceCollection services)
    { }

    /// <summary>
    /// Adds routes for the <see cref="IEndpointGroup"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> the routes should be added to.</param>
    void AddRoutes(WebApplication app);
}