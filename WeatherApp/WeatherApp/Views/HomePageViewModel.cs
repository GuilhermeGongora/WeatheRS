using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Views;
using Xamarin.Essentials;

public class HomePageViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CityWeather> Cities { get; set; }
    public ICommand SearchCommand { get; private set; }

    private CityWeather _currentLocationWeather;
    public CityWeather CurrentLocationWeather
    {
        get => _currentLocationWeather;
        set
        {
            _currentLocationWeather = value;
            OnPropertyChanged();
        }
    }

    private CityWeather _newCityWeather;
    public CityWeather NewCityWeather
    {
        get => _newCityWeather;
        set
        {
            _newCityWeather = value;
            OnPropertyChanged();
        }
    }

    public HomePageViewModel()
    {
        Cities = new ObservableCollection<CityWeather>();
        LoadCurrentLocationWeather().Wait(); // Aguarda a conclusão da tarefa
    }

    // Evento exigido pela interface INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    // Método para disparar o evento PropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Método para carregar os dados da cidade atual via geolocalização
    public async Task LoadCurrentLocationWeather()
    {
        CurrentLocationWeather = await GetWeatherForCurrentLocation();
    }

    // Método para obter os dados meteorológicos da localização atual
    private async Task<CityWeather> GetWeatherForCurrentLocation()
    {
        try
        {
            // Obter a localização atual do usuário
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location != null)
            {
                // Chamar a API da OpenWeatherMap para obter os dados de clima
                var apiKey = "0254e13028fbf335e64c91ff361ce46f"; // Substitua pela sua chave da API OpenWeatherMap
                var lat = location.Latitude;
                var lon = location.Longitude;
                var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var forecastData = JsonConvert.DeserializeObject<ForecastInfo>(response);

                    // Pegue o primeiro item da lista de previsão
                    var weatherData = forecastData.list.FirstOrDefault();

                    if (weatherData != null)
                    {
                        return new CityWeather
                        {
                            CityName = forecastData.city.name,
                            WeatherDescription = weatherData.weather.FirstOrDefault()?.description,
                            Temperature = $"{weatherData.main.temp}°C",
                            Humidity = $"{weatherData.main.humidity}%",
                            Pressure = $"{weatherData.main.pressure} hPa",
                            Wind = $"{weatherData.wind.speed} m/s",
                            Cloudiness = $"{weatherData.clouds.all}%",
                            Date = DateTimeOffset.FromUnixTimeSeconds(weatherData.dt).DateTime.ToString("dd/MM/yyyy HH:mm"),
                            Icon = $"http://openweathermap.org/img/wn/{weatherData.weather.FirstOrDefault()?.icon}.png"
                        };
                    }
                    else
                    {
                        // Caso a lista esteja vazia
                        return new CityWeather
                        {
                            CityName = "Dados não disponíveis",
                            WeatherDescription = "N/A",
                            Temperature = "N/A",
                            Humidity = "N/A",
                            Pressure = "N/A",
                            Wind = "N/A",
                            Cloudiness = "N/A",
                            Date = "N/A",
                            Icon = "N/A"
                        };
                    }
                }
            }
            else
            {
                // Caso a localização não esteja disponível
                return new CityWeather
                {
                    CityName = "Localização não encontrada",
                    WeatherDescription = "N/A",
                    Temperature = "N/A",
                    Humidity = "N/A",
                    Pressure = "N/A",
                    Wind = "N/A",
                    Cloudiness = "N/A",
                    Date = "N/A",
                    Icon = "N/A"
                };
            }
        }
        catch (Exception ex)
        {
            // Tratamento de erro
            return new CityWeather
            {
                CityName = "Erro ao obter localização",
                WeatherDescription = "N/A",
                Temperature = "N/A",
                Humidity = "N/A",
                Pressure = "N/A",
                Wind = "N/A",
                Cloudiness = "N/A",
                Date = "N/A",
                Icon = "N/A"
            };
        }
    }


    // Adiciona uma nova cidade à lista
    public void AddCity(CityWeather city)
    {
        if (!Cities.Contains(city))
            Cities.Add(city);
    }

    // Novo método para carregar os dados de uma cidade pesquisada
    public async Task LoadNewCityWeather(string cityName)
    {
        NewCityWeather = await GetWeatherForCity(cityName);
    }

    private async Task<CityWeather> GetWeatherForCity(string cityName)
    {
        try
        {
            // Chamar a API da OpenWeatherMap para obter os dados de clima
            var apiKey = ""; // Substitua pela sua chave da API OpenWeatherMap
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var weatherData = JsonConvert.DeserializeObject<WeatherInfo>(response);

                return new CityWeather
                {
                    CityName = weatherData.name,
                    WeatherDescription = weatherData.weather[0].description,
                    Temperature = $"{weatherData.main.temp}°C"
                };
            }
        }
        catch (Exception ex)
        {
            // Tratamento de erro
            return new CityWeather
            {
                CityName = "Erro ao obter dados",
                WeatherDescription = "N/A",
                Temperature = "N/A"
            };
        }
    }
}
