// WeatherDetailPage.xaml.cs
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.Views
{
    public partial class WeatherDetailPage : ContentPage
    {
        public WeatherDetailPage(SavedCity savedCity)
        {
            InitializeComponent();

            // Exibir informações na UI
            cityTxt.Text = savedCity.Name.ToUpperInvariant();
            temperatureTxt.Text = $"{savedCity.Temperature.ToString("F0")}";
            descriptionTxt.Text = $"{savedCity.Description}";
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
    }
}
