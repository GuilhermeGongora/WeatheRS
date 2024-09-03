using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using WeatherApp.Helper;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HttpClient _httpClient;

        public HomePage()
        {
           

            InitializeComponent();
            GetCoordinates();
            // Ajusta o tamanho do mapa
        }
       

        private string Location { get; set; } = "Ireland";
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private void OnMenuButtonClicked(Object sender, EventArgs args)
        {
           
        }
        private async void GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);

                    GetWeatherInfo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

       

        private string CorrectLocationName(string name)
        {
            var corrections = new Dictionary<string, string>
            {
                { "Ak”yar", "Aviação" },
                // Adicione mais correções conforme necessário
            };

            if (corrections.ContainsKey(name))
            {
                return corrections[name];
            }

            return name;
        }

        private async void GetWeatherInfo()
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Location}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";

            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    // Log da resposta JSON para depuração
                    Console.WriteLine($"Weather API Response: {result.Response}");

                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);

                    if (weatherInfo != null)
                    {
                        descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                        iconImg.Source = $"w{weatherInfo.weather[0].icon}";

                        // Verificar se a umidade e a temperatura são corretas
                        Console.WriteLine($"Humidity: {weatherInfo.main.humidity}%");
                        Console.WriteLine($"Temperature: {weatherInfo.main.temp}°C");

                        cityTxt.Text = CorrectLocationName(weatherInfo.name).ToUpperInvariant();
                        temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                        humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                        pressureTxt.Text = $"{weatherInfo.main.pressure} hPa";
                        windTxt.Text = $"{weatherInfo.wind.speed} m/s";
                        cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

                        // Converte para UTC
                        var utcDateTime = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.dt).UtcDateTime;

                        // Aplica o fuso horário fornecido pela API (em segundos)
                        var localDateTime = utcDateTime.AddSeconds(weatherInfo.timezone);

                        dateTxt.Text = localDateTime.ToString("dddd, MMM dd").ToUpper();
                        GetForecast();
                    }
                }
                catch (Exception ex)
                {
                    // Tratar exceção
                    Console.WriteLine($"Error processing weather data: {ex.Message}");
                    await DisplayAlert("Weather Info", "Error processing weather data", "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }

        private async Task<string> GetCity(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if (currentPlace != null)
            {
                // Priorize AdminArea ou SubAdminArea se estiver disponível para capturar a cidade corretamente
                return !string.IsNullOrEmpty(currentPlace.SubAdminArea) ? currentPlace.SubAdminArea : currentPlace.AdminArea;
            }
            return null;
        }

        private async void GetForecast()
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Location}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";
            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var forecastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<List> allList = new List<List>();

                    foreach (var list in forecastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }

                    var cultureInfo = new CultureInfo("pt-BR");

                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd", cultureInfo);
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM", cultureInfo);
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0");

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd", cultureInfo);
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM", cultureInfo);
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoTxt.Text = allList[1].main.temp.ToString("0");

                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd", cultureInfo);
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM", cultureInfo);
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeTxt.Text = allList[2].main.temp.ToString("0");

                    dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd", cultureInfo);
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM", cultureInfo);
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourTxt.Text = allList[3].main.temp.ToString("0");

                    dayFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dddd", cultureInfo);
                    dateFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dd MMM", cultureInfo);
                    iconFiveImg.Source = $"w{allList[4].weather[0].icon}";
                    tempFiveTxt.Text = allList[4].main.temp.ToString("0");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }

    }
}
