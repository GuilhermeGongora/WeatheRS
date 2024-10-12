using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using WeatherApp.Models;
using WeatherApp.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherApp.Views
{
    public partial class LocationSearchPage : ContentPage
    {
        public LocationSearchPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCitiesAsync(); // Carrega as cidades ao aparecer
        }

        // Carrega as cidades do banco de dados e atualiza as informações
        private async Task LoadCitiesAsync()
        {
            // Recupera as cidades salvas do banco de dados
            var cities = await App.Database.GetCitiesAsync();

            // Atualiza os dados das cidades antes de exibir
            await UpdateCitiesData(cities);

            // Define a lista de cidades no ListView
            citiesListView.ItemsSource = cities;
        }

        private async void OnCitySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is SavedCity selectedCity)
            {
                // Navega para a página de detalhes da cidade selecionada
                await Navigation.PushAsync(new WeatherDetailPage(selectedCity));
                ((ListView)sender).SelectedItem = null; // Desmarca a seleção
            }
        }

        private async void OnReloadButtonClicked(object sender, EventArgs e)
        {
            await LoadCitiesAsync(); // Recarrega a lista de cidades
        }

        // Método para atualizar os dados das cidades
        private async Task UpdateCitiesData(List<SavedCity> cities)
        {
            foreach (var city in cities)
            {
                try
                {
                    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city.Name}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric&lang=pt";
                    var result = await ApiCaller.Get(url);

                    if (result.Successful)
                    {
                        var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);

                        if (weatherInfo != null)
                        {
                            // Atualiza as informações da cidade
                            city.Temperature = weatherInfo.main.temp;
                            city.Humidity = weatherInfo.main.humidity;
                            city.Pressure = weatherInfo.main.pressure; // Atualiza a pressão
                            city.WindSpeed = weatherInfo.wind.speed;
                            city.Cloudiness = weatherInfo.clouds.all; // Atualiza a nebulosidade
                            city.Icon = $"w{weatherInfo.weather[0].icon}"; // Atualiza o ícone
                            city.Description = weatherInfo.weather[0].description; // Atualiza a descrição
                            city.Date = DateTime.Now; // Atualiza a data

                            // Atualiza o banco de dados
                            await App.Database.SaveCityAsync(city);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating city data: {ex.Message}");
                }
            }
        }

        // Método para lidar com a exclusão de uma cidade
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var cityToDelete = (SavedCity)menuItem.CommandParameter;

            // Confirmação de exclusão
            var confirmed = await DisplayAlert("Delete", $"Are you sure you want to delete {cityToDelete.Name}?", "Yes", "No");
            if (confirmed)
            {
                // Exclui a cidade do banco de dados
                await App.Database.DeleteCityAsync(cityToDelete);
                await LoadCitiesAsync(); // Recarrega a lista de cidades
            }
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            // Verifica se o campo de pesquisa não está vazio
            if (string.IsNullOrWhiteSpace(searchBar.Text))
            {
                await DisplayAlert("Aviso!", "Digite um nome de uma localidade.", "OK");
                return;
            }

            var url = $"https://api.openweathermap.org/data/2.5/weather?q={searchBar.Text.Trim()}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric&lang=pt";

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
                            Date = DateTime.UtcNow,
                            Description = weatherInfo.weather[0].description // Adiciona a descrição do clima
                        };

                        // Salva a cidade no banco de dados
                        await App.Database.SaveCityAsync(savedCity);

                        // Navega para a página de detalhes da cidade
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
