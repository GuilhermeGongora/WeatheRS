using System;
using WeatherApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new FlyoutAppMenuWeather(); // Defina a página inicial aqui

        }

        protected override void OnStart()
        {
            Sharpnado.Tabs.Initializer.Initialize(true, false);
            base.OnStart();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
