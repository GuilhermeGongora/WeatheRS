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
            var newsArticles = await _newsService.GetNewsAsync();
            NewsCollectionView.ItemsSource = newsArticles;
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
