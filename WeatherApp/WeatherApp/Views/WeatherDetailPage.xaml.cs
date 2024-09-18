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
            temperatureTxt.Text = $"{savedCity.Temperature.ToString("F0")}C";
            humidityTxt.Text = $"{savedCity.Humidity}%";
            pressureTxt.Text = $"{savedCity.Pressure} hPa";
            windTxt.Text = $"{savedCity.WindSpeed.ToString("F0")} m/s";
            cloudinessTxt.Text = $"{savedCity.Cloudiness}%";
            dateTxt.Text = savedCity.Date.ToString("dddd, MMM dd").ToUpper();
            iconImg.Source = $"w{savedCity.Icon}";
        }
    }
}
