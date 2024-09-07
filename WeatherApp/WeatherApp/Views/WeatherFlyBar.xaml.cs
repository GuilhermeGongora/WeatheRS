using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherFlyBar : ContentPage
    {
        public WeatherFlyBar()
        {
            InitializeComponent();
        }

        private async void OnIconClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LocationSearchPage());
        }
    }
}