using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IGetWeatherService, WeatherService>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});
var app = builder.Build();

app.MapGet("/", () => Results.Content("""
    <h1>Погода в разных городах России</h1>
    <p>Использование: /weather/{city}</p>
    <h3>Примеры:</h3>
    <ul>
        <li><a href='/weather/moscow'>Москва</a></li>
        <li><a href='/weather/vladimir'>Владимир</a></li>
        <li><a href='/weather/kazan'>Казань</a></li>
        <li><a href='/weather/ufa'>Уфа</a></li>
    </ul>
    """, "text/html; charset=utf-8"));


app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.ContentType = "text/html;charset=utf-8";
        await context.HttpContext.Response.WriteAsync("""
            <h1>Город не найден</h1>
            <a href='/'>На главную</a>
            """);
    }
});


app.MapGet("/weather/{city}", ([FromServices] IGetWeatherService weatherService, string city) =>
{
var weather = weatherService.GetWeather(city);
var result = new
{
    city = weather.City,
    temperature = weather.Temperature
};
return Results.Json(result);
});

app.Run();

public interface IGetWeatherService
{
    WeatherResponse GetWeather(string city);
};
public record WeatherResponse(string City, string Temperature);

public class WeatherService : IGetWeatherService
{
    private readonly Dictionary<string, string> weatherDictionary;
    public WeatherService()
    {
        weatherDictionary = new Dictionary<string, string>
        {
            ["moscow"] = "+32℃",
            ["vladimir"] = "+24℃",
            ["kazan"] = "+30℃",
            ["ufa"] = "+23℃",
        };
    }
    public WeatherResponse GetWeather(string city)
    {
        weatherDictionary.TryGetValue(city, out var weatherInfo);
        return new WeatherResponse(city, weatherInfo ?? "Не найдено");
    }
}


