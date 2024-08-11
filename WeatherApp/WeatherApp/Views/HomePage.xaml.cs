using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using WeatherApp.Helper;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HttpClient _httpClient;

        public HomePage()
        {
            InitializeComponent();
            GetCoordinates();
            _httpClient = new HttpClient();
            LoadSatelliteMap();
        }
        private async void LoadSatelliteMap()
        {
            // URL para obter a imagem de satélite (isso é apenas um exemplo e pode precisar de ajustes)
            var url = "http://tile.openweathermap.org/map/satellite/{z}/{x}/{y}.png?appid=0254e13028fbf335e64c91ff361ce46f";

            // Obtendo o stream da imagem (isso pode não ser necessário dependendo de como você deseja usar a imagem)
            var imageStream = await _httpClient.GetStreamAsync(url);
            var satelliteImage = ImageSource.FromStream(() => imageStream);

            // Exemplo de coordenadas fixas para latitude e longitude
            double latitude = -23.5505; // Substitua pelo valor desejado
            double longitude = -46.6333; // Substitua pelo valor desejado

            // Configuração básica do mapa
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromMiles(10)));

            // Adicionando um pin
            var pin = new Pin
            {
                Label = "Local",
                Position = new Position(latitude, longitude),
                Type = PinType.Place
            };
            myMap.Pins.Add(pin);
        }

        private string Location { get; set; } = "Ireland";
        public double Latitude { get; set; }    
        public double Longitude { get; set; }   

        private async void GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);

                    GetWeatherInfo();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private async Task<string> GetCity(Location location)
        {

            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault(); 

            if (currentPlace != null) {
                return $"{currentPlace.Locality},{currentPlace.CountryName}";

            }
            return null ;
        }
        private string CorrectLocationName(string name)
        {
            var corrections = new Dictionary<string, string>
    {
        { "Ak”yar", "Aviação" },
        // Adicione mais correções conforme necessário
    };

            if (corrections.ContainsKey(name))
            {
                return corrections[name];
            }

            return name;
        }

        private async void GetWeatherInfo()
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Location}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";

            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = CorrectLocationName(weatherInfo.name).ToUpperInvariant();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                    humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                    pressureTxt.Text = $"{weatherInfo.main.pressure} hpa";
                    windTxt.Text = $"{weatherInfo.wind.speed} m/s";
                    cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

                    // Converte para UTC
                    var utcDateTime = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.dt).UtcDateTime;

                    // Aplica o fuso horário fornecido pela API (em segundos)
                    var localDateTime = utcDateTime.AddSeconds(weatherInfo.timezone);

                    dateTxt.Text = localDateTime.ToString("dddd, MMM dd").ToUpper();
                    GetForecast();
                }
                catch (Exception ex)
                {
                    // Tratar exceção
                }
            }
            else
            {
                await DisplayAlert("Weather Info", "No weather information found", "OK");
            }
        }

        private async void GetForecast()
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={Location}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric";
            var result = await ApiCaller.Get(url) ;
            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    List<List> allList = new List<List>();

                    foreach (var list in forcastInfo.list)
                    {
                        //var date = DateTime.ParseExact(list.dt_txt, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }

                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM");
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0");

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM");
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoTxt.Text = allList[1].main.temp.ToString("0");

                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM");
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeTxt.Text = allList[2].main.temp.ToString("0");

                    dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM");
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourTxt.Text = allList[3].main.temp.ToString("0");
                    
                    dayFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dddd");
                    dateFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dd MMM");
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
