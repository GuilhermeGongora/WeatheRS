using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Helper;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationForecast : PopupPage
    {
        private string Location { get; set; } = "Ireland"; // Localização padrão
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationForecast(string cityName)
        {
            InitializeComponent();
            // Agora chamamos GetForecast passando o nome da cidade
            GetForecast(cityName);
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private async void GetForecast(string cityName)
        {
            // URL da API utilizando o nome da cidade
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid=0254e13028fbf335e64c91ff361ce46f&units=metric&lang=pt";

            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    var forecastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);
                    List<List> allList = new List<List>();

                    // Filtrar o forecast para pegar os dias à meia-noite
                    foreach (var list in forecastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);
                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                        {
                            allList.Add(list);
                        }
                    }

                    var cultureInfo = new CultureInfo("pt-BR");

                    // Função para abreviar os dias da semana
                    string AbreviarDiaDaSemana(string diaCompleto)
                    {
                        return char.ToUpper(diaCompleto[0]) + diaCompleto.Substring(1, 2);
                    }

                    // Atualizando os labels com os dias abreviados e ícones
                    dayOneTxt.Text = AbreviarDiaDaSemana(DateTime.Parse(allList[0].dt_txt).ToString("dddd", cultureInfo));
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM", cultureInfo);
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneTxt.Text = allList[0].main.temp.ToString("0") + "°C";

                    dayTwoTxt.Text = AbreviarDiaDaSemana(DateTime.Parse(allList[1].dt_txt).ToString("dddd", cultureInfo));
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM", cultureInfo);
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoTxt.Text = allList[1].main.temp.ToString("0") + "°C";

                    dayThreeTxt.Text = AbreviarDiaDaSemana(DateTime.Parse(allList[2].dt_txt).ToString("dddd", cultureInfo));
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM", cultureInfo);
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeTxt.Text = allList[2].main.temp.ToString("0") + "°C";

                    dayFourTxt.Text = AbreviarDiaDaSemana(DateTime.Parse(allList[3].dt_txt).ToString("dddd", cultureInfo));
                    dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM", cultureInfo);
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourTxt.Text = allList[3].main.temp.ToString("0") + "°C";

                    dayFiveTxt.Text = AbreviarDiaDaSemana(DateTime.Parse(allList[4].dt_txt).ToString("dddd", cultureInfo));
                    dateFiveTxt.Text = DateTime.Parse(allList[4].dt_txt).ToString("dd MMM", cultureInfo);
                    iconFiveImg.Source = $"w{allList[4].weather[0].icon}";
                    tempFiveTxt.Text = allList[4].main.temp.ToString("0") + "°C";
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
