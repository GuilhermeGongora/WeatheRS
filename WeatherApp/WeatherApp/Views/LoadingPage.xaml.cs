using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeatherApp.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadMainContent(); // Chama o método que carrega o conteúdo principal
        }

        private async Task LoadMainContent()
        {
            // Simulação de carregamento de dados
            await Task.Delay(3000); // Espera 3 segundos

            // Navega para a página principal do aplicativo
            Application.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}
