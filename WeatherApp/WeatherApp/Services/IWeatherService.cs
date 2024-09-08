using System.Threading.Tasks;
using WeatherApp.Models;

public interface IWeatherService
{
    Task<WeatherInfo> GetWeatherAsync(string location);
    Task<ForecastInfo> GetForecastAsync(string location);
}
