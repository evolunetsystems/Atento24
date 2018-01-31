using System.Threading.Tasks;
using atento24.Recursos;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_registro : PopupPage
    {
        public pg_registro()
        {
            InitializeComponent();
            string s_plural = VarGlobal.alerta_can_registro > 1 ? "s " : " ";

            lb_conteo.Text = "Tiene" + s_plural + VarGlobal.alerta_can_registro.ToString().Trim() + " Registro" + s_plural + " por sincronizar";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HidePopup();
        }

        private async void HidePopup()
        {
            await Task.Delay(4000);
            await PopupNavigation.RemovePageAsync(this);
        }
    }
}
