using Xamarin.Forms;
using System.Collections.Generic;
using WeatherApp.Models;

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

            // Recupera as cidades salvas do banco de dados
            var cities = await App.Database.GetCitiesAsync();

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
    }
}
