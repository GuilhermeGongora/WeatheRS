using Xamarin.Forms;

namespace WeatherApp.Views
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rotas de navegação, se necessário
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }
    }
}
