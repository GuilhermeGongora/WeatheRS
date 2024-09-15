// LocationSearchPage.xaml.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WeatherApp.Helper;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.Views
{
    public partial class LocationSearchPage : ContentPage
    {
        private List<SavedCity> savedCities = new List<SavedCity>();

        public LocationSearchPage()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={searchBar.Text}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";

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
                        // Adicionar cidade à lista
                        var savedCity = new SavedCity
                        {
                            Name = CorrectLocationName(weatherInfo.name),
                            Temperature = weatherInfo.main.temp,
                            Humidity = weatherInfo.main.humidity,
                            Pressure = weatherInfo.main.pressure,
                            WindSpeed = weatherInfo.wind.speed,
                            Cloudiness = weatherInfo.clouds.all,
                            Icon = $"w{weatherInfo.weather[0].icon}",
                            Date = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.dt).UtcDateTime
                                .AddSeconds(weatherInfo.timezone)
                        };

                        savedCities.Add(savedCity);

                        // Navegar para a página de detalhes
                        await Navigation.PushAsync(new WeatherDetailPage(savedCity));
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

        private async void OnViewSavedCitiesButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedCitiesPage(savedCities));
        }

        private string CorrectLocationName(string locationName)
        {
            // Sua implementação para corrigir o nome da localização
            return locationName;
        }
    }
}
