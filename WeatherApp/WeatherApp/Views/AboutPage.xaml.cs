using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();
		}
        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var instagramUrl = (string)button.CommandParameter; // Obtém o URL do CommandParameter

            await Launcher.OpenAsync(instagramUrl);
        }


    }
}