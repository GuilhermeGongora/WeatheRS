using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace WeatherApp.Views
{
    public class SlideUpAnimation : BaseAnimation
    {
        public override async Task Appearing(View view, PopupPage page)
        {
            // Configura o estado inicial do view antes da animação começar
            view.TranslationY = view.Height;
            // Animação para deslizar para cima
            await view.TranslateTo(0, 0, 300, Easing.SpringOut);
        }

        public override async Task Disappearing(View view, PopupPage page)
        {
            // Animação para deslizar para baixo ao desaparecer
            await view.TranslateTo(0, view.Height, 300, Easing.SpringOut);
        }
        
        public override void Disposing(View view, PopupPage page)
        {
            // Adicione qualquer lógica de limpeza, se necessário
        }

        public override void Preparing(View view, PopupPage page)
        {
            // Configura o estado inicial do view antes da animação começar
            view.TranslationY = view.Height;
        }
    }
}
