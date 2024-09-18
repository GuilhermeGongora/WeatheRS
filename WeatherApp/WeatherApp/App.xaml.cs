using System;
using System.IO;
using WeatherApp.Helper;
using WeatherApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    public partial class App : Application
    {
        static DatabaseService database;

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
        public static DatabaseService Database
        {
            get
            {
                if (database == null)
                {
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WeatherApp.db3");
                    database = new DatabaseService(dbPath);
                }
                return database;
            }
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
