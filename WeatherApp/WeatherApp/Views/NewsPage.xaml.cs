using System;
using WeatherApp.Helper;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace WeatherApp.Views
{
    public partial class NewsPage : ContentPage
    {
        private readonly NewsService _newsService = new NewsService();

        public NewsPage()
        {
            InitializeComponent();
            LoadNews();
        }

        private async void LoadNews()
        {
            try
            {
                // Mostra um indicador de carregamento (opcional)
                IsBusy = true;

                var newsArticles = await _newsService.GetNewsAsync();

                if (newsArticles != null)
                {
                    NewsCollectionView.ItemsSource = newsArticles;
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível carregar as notícias.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Houve um problema ao carregar as notícias: " + ex.Message, "OK");
            }
            finally
            {
                IsBusy = false; // Oculta o indicador de carregamento
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedArticle = e.CurrentSelection[0] as NewsArticle;
                if (selectedArticle != null)
                {
                    await Launcher.OpenAsync(new Uri(selectedArticle.Url));
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}
