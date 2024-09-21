using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Helper;
using WeatherApp.Models;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "0254e13028fbf335e64c91ff361ce46f&units=metric";

    public WeatherService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<WeatherInfo> GetWeatherAsync(string location)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={ApiKey}&units=metric";
        var result = await ApiCaller.Get(url);

        if (result.Successful)
        {
            return JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
        }

        return null;
    }

    public async Task<ForecastInfo> GetForecastAsync(string location)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast?q={location}&appid={ApiKey}&units=metric";
        var result = await ApiCaller.Get(url);

        if (result.Successful)
        {
            return JsonConvert.DeserializeObject<ForecastInfo>(result.Response);
        }

        return null;
    }
}
