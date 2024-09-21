using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using WeatherApp.Models;
using WeatherApp.Helper;

namespace WeatherApp.Views
{
    public partial class LocationSearchPage : ContentPage
    {
        public LocationSearchPage()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={searchBar.Text.Trim()}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";
            
            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);

                    if (weatherInfo != null)
                    {
                        // Cria uma nova cidade com base nos dados da API
                        var savedCity = new SavedCity
                        {
                            Name = weatherInfo.name,
                            Temperature = weatherInfo.main.temp,
                            Humidity = weatherInfo.main.humidity,
                            Pressure = weatherInfo.main.pressure,
                            WindSpeed = weatherInfo.wind.speed,
                            Cloudiness = weatherInfo.clouds.all,
                            Icon = $"w{weatherInfo.weather[0].icon}",
                            Date = DateTime.UtcNow
                        };

                        // Salva a cidade no banco de dados
                        await App.Database.SaveCityAsync(savedCity);

                        // Navega para a página de detalhes da cidade ou faz outra ação
                        await Navigation.PushAsync(new WeatherDetailPage(savedCity));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing weather data: {ex.Message}");
                    await DisplayAlert("Weather Info", "Error processing weather data", "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }

        private async void OnViewSavedCitiesButtonPressed(object sender, EventArgs e)
        {
            // Navega para a página que lista as cidades salvas
            await Navigation.PushAsync(new SavedCitiesPage());
        }
    }
}
