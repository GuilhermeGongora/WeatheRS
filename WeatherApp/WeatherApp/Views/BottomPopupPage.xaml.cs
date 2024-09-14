using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Helper;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomPopupPage : PopupPage
    {
        private string Location { get; set; } = "Ireland";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public BottomPopupPage()
        {
            InitializeComponent();
            // Configurar a animação personalizada
            GetForecast();
            GetCoordinates();

            this.Animation = new SlideUpAnimation();
        }
        private async Task<string> GetCity(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();

            if (currentPlace != null)
            {
                // Priorize AdminArea ou SubAdminArea se estiver disponível para capturar a cidade corretamente
                return !string.IsNullOrEmpty(currentPlace.SubAdminArea) ? currentPlace.SubAdminArea : currentPlace.AdminArea;
            }
            return null;
        }
        private async void GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }


        private async void GetForecast()
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Location}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";
            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var forecastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<List> allList = new List<List>();

                    foreach (var list in forecastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }

                    var cultureInfo = new CultureInfo("pt-BR");

                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd", cultureInfo);
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM", cultureInfo);
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0");

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd", cultureInfo);
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM", cultureInfo);
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoTxt.Text = allList[1].main.temp.ToString("0");

                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd", cultureInfo);
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM", cultureInfo);
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeTxt.Text = allList[2].main.temp.ToString("0");

                    dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd", cultureInfo);
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM", cultureInfo);
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourTxt.Text = allList[3].main.temp.ToString("0");

                    dayFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dddd", cultureInfo);
                    dateFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dd MMM", cultureInfo);
                    iconFiveImg.Source = $"w{allList[4].weather[0].icon}";
                    tempFiveTxt.Text = allList[4].main.temp.ToString("0");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }

    }

}