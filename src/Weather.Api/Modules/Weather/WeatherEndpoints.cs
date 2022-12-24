namespace Weather.Api.Modules.Weather;

internal sealed class WeatherEndpointGroup : IEndpointGroup
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public void AddRoutes(WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/weather")
            .WithTags("weather");

        group.MapGet("/", GetForecast)
            .WithName("GetWeatherForecast")
            .WithDescription("Gets the 5 day forecast")
            .CacheOutput();
    }

    private static WeatherForecast[] GetForecast(HttpContext context) =>
        Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
}