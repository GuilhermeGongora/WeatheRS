using Rg.Plugins.Popup.Services;
using System;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.Views
{
    public partial class WeatherDetailPage : ContentPage
    {
        private SavedCity _savedCity; // Variável de classe

        public WeatherDetailPage(SavedCity savedCity)
        {
            InitializeComponent();
            _savedCity = savedCity; // Armazena o objeto na variável

            // Exibir informações na UI
            cityTxt.Text = savedCity.Name.ToUpperInvariant();
            temperatureTxt.Text = $"{savedCity.Temperature.ToString("F0")}";
            descriptionTxt.Text = $"{savedCity.Description}".ToUpperInvariant();
            humidityTxt.Text = $"{savedCity.Humidity}%";
            pressureTxt.Text = $"{savedCity.Pressure} hPa";
            windTxt.Text = $"{savedCity.WindSpeed.ToString("F0")} m/s";
            cloudinessTxt.Text = $"{savedCity.Cloudiness}%";
            dateTxt.Text = savedCity.Date.ToString("dddd, MMM dd").ToUpper();
            // Verifica se os dois primeiros caracteres são iguais, e remove o primeiro se for "w"
            string iconName = savedCity.Icon.StartsWith("ww") ? savedCity.Icon.Substring(1) : savedCity.Icon;

            // Atualiza a fonte da imagem com o ícone correto
            iconImg.Source = iconName;
        }

        private async void OnOpenPopupButtonClicked(object sender, EventArgs e)
        {
            // Usa a variável de classe
            string cityName = _savedCity.Name;

            // Passa apenas o nome da cidade para LocationForecast
            await PopupNavigation.Instance.PushAsync(new LocationForecast(cityName));
        }
    }
}
