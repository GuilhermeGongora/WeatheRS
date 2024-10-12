using Xamarin.Forms;
using System.Collections.Generic;
using WeatherApp.Models;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WeatherApp.Helper;
using System;

namespace WeatherApp.Views
{
    public partial class SavedCitiesPage : ContentPage
    {
        public SavedCitiesPage()
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
                            // Atualiza todas as informações da cidade
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
    }
}
