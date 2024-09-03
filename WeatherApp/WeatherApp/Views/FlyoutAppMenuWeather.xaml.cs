using System;
using Xamarin.Forms;

namespace WeatherApp.Views
{
    public partial class FlyoutAppMenuWeather : FlyoutPage
    {
        public FlyoutAppMenuWeather()
        {
            InitializeComponent();

            // Verifique se a lista de itens está sendo preenchida corretamente
            flyout.listview.ItemSelected += OnSelectedItem;
        }

        private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutItemPage;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.targetPage));
                flyout.listview.SelectedItem = null;
                IsPresented = false; // Fecha o menu Flyout
            }
        }
    }
}
