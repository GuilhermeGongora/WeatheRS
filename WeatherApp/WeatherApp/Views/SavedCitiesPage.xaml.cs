// SavedCitiesPage.xaml.cs
using Xamarin.Forms;
using System.Collections.Generic;

namespace WeatherApp.Views
{
    public partial class SavedCitiesPage : ContentPage
    {
        public SavedCitiesPage(List<SavedCity> cities)
        {
            InitializeComponent();

            // Definir a lista de cidades
            citiesListView.ItemsSource = cities;
        }

        private async void OnCitySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is SavedCity selectedCity)
            {
                // Navegar para a página de detalhes da cidade selecionada
                await Navigation.PushAsync(new WeatherDetailPage(selectedCity));
                ((ListView)sender).SelectedItem = null; // Desmarcar a seleção
            }
        }
    }
}
