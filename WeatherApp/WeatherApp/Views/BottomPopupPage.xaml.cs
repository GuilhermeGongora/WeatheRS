using Rg.Plugins.Popup.Pages;
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
    public partial class BottomPopupPage : PopupPage
    {
        public BottomPopupPage()
        {
            InitializeComponent();
            // Configurar a animação personalizada
            this.Animation = new SlideUpAnimation();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}