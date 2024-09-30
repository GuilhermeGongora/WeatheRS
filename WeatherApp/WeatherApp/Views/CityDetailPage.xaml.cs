using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using WeatherApp.Helper;
using WeatherApp.Models;

namespace WeatherApp.Views
{
    public partial class CityDetailPage : ContentPage
    {
        private string _cityName;

        public CityDetailPage(string cityName)
        {
            InitializeComponent();
            _cityName = cityName;
            LoadWeatherInfo();
        }

        private async void LoadWeatherInfo()
        {
            // Aqui você pode usar o mesmo método que você tem na HomePage para obter as informações
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={_cityName}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                cityTxt.Text = weatherInfo.name.ToUpperInvariant();
                temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                pressureTxt.Text = $"{weatherInfo.main.pressure} hPa";
                windTxt.Text = $"{weatherInfo.wind.speed} m/s";
                cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";
                // Carregue outros elementos como temperatura, umidade, etc.
            }
        }
    }
}
