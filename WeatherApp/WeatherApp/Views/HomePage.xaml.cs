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
using System.Windows.Input;
using System.Collections.ObjectModel;
using WeatherApp.ViewModels;
using Rg.Plugins.Popup.Services;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HttpClient _httpClient;
        public ICommand SearchCommand { get; set; }
        public ObservableCollection<string> SearchResults { get; set; }


        public HomePage()
        {
            InitializeComponent();

            // Inicializar a lista de resultados
            LoadData();

            BindingContext = this;
            GetCoordinates();
            // Ajusta o tamanho do mapa
        }
        private async void LoadData()
        {
            // Obtenha a cidade atual por geolocalização (implemente seu método aqui)
            var currentCity = await GetCurrentCity();

            // Obtenha as cidades salvas do banco de dados
            var savedCities = await App.Database.GetCitiesAsync();

            // Crie uma lista que inclui a cidade atual
            var allCities = new List<SavedCity> { currentCity };
            allCities.AddRange(savedCities);

            // Defina a lista de cidades no CarouselView
        }

        private async Task<SavedCity> GetCurrentCity()
        {
            // Obtém a localização atual
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                // Usa a localização para obter o nome da cidade
                var cityName = await GetCity(location);

                // Obtém as informações meteorológicas da cidade
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric&lang=pt";
                var result = await ApiCaller.Get(url);

                if (result.Successful)
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    if (weatherInfo != null)
                    {
                        return new SavedCity
                        {
                            Name = weatherInfo.name,
                            Temperature = weatherInfo.main.temp,
                            Humidity = weatherInfo.main.humidity,
                            Pressure = weatherInfo.main.pressure,
                            WindSpeed = weatherInfo.wind.speed,
                            Cloudiness = weatherInfo.clouds.all,
                            Date = DateTime.UtcNow // ou qualquer data que você quiser
                        };
                    }
                }
            }

            return null; // Retorna null se não conseguir obter a cidade atual
        }


        private async void OnOpenPopupButtonClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new BottomPopupPage());
        }

        private void OnSearch(string query)
        {
            // Lógica de pesquisa
            if (!string.IsNullOrWhiteSpace(query))
            {
                DisplayAlert("Search", $"You searched for: {query}", "OK");
            }
        }
        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
            // Verifique se o pai da página é um FlyoutPage
            if (this.Parent is FlyoutPage flyoutPage)
            {
                flyoutPage.IsPresented = true; // Abre o menu Flyout
            }
        }


        private string Location { get; set; } = "Ireland";
        public double Latitude { get; set; }
        public double Longitude { get; set; }



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
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Location}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric&lang=pt";

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
    }
}

      