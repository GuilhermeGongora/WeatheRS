using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationSearchPage : ContentPage
	{
		public LocationSearchPage ()
		{
			InitializeComponent ();
		}
        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string cityName = searchBar.Text;
            if (!string.IsNullOrEmpty(cityName))
            {
                var weatherData = await GetWeatherDataAsync(cityName);
                if (weatherData != null)
                {
                    // Adiciona a nova cidade à página inicial
                    MessagingCenter.Send(this, "AddCity", weatherData);
                }
            }
        }

        private async Task<WeatherInfo> GetWeatherDataAsync(string cityName)
        {
            string apiKey = "YOUR_API_KEY";
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherInfo>(content);
                    return weatherData;
                }
                else
                {
                    // Tratar erro de requisição
                    return null;
                }
            }
        }

    }
}