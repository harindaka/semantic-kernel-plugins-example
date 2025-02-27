using System.ComponentModel;
using Microsoft.SemanticKernel;

public sealed record WeatherInfo
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Condition { get; set; } = string.Empty;
}

public class WeatherPlugin 
{
    [KernelFunction("get_current_weather")]
    [Description("Retrieves the current weather for a given city including temperature and condition")]
    public Task<WeatherInfo> GetCurrentWeather(string city)
    {        
        // Mock weather data
        return Task.FromResult(new WeatherInfo
        {
            City = city,
            Temperature = 22.5,
            Condition = "Sunny"
        });
    }
}