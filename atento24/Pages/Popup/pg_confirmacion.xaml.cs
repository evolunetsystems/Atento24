using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_confirmacion : PopupPage
    {
        TaskCompletionSource<bool> _tcs = null;

        public pg_confirmacion(ent_mensaje entidad)
        {
            InitializeComponent();
            entidad.ico_mensaje = "fa-question-circle";

            switch (entidad.tip_mensaje)
            {
                case "INF":
                    //entidad.ico_mensaje = "fa-info-circle";
                    entidad.col_mensaje = "#0199DC";
                    break;
                case "WAR":
                    //entidad.ico_mensaje = "fa-exclamation-triangle";
                    entidad.col_mensaje = "#FF8000";
                    break;
                case "ERR":
                    //entidad.ico_mensaje = "fa-times-circle";
                    entidad.col_mensaje = "#FF0000";
                    break;
            }
            AsignarValores(entidad);

        }

        private void AsignarValores(ent_mensaje entidad)
        {
            iconSincro.Text = entidad.ico_mensaje;
            lb_titulo.Text = entidad.tit_mensaje;
            lb_mensaje.Text = entidad.tex_mensaje;
            sl_confirmacion.BackgroundColor = Color.FromHex(entidad.col_mensaje);
        }

        private async void SiGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync();
            _tcs?.SetResult(true);
        }

        private async void NoGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync();
            _tcs?.SetResult(false);
        }

        public async Task<bool> Show()
        {
            _tcs = new TaskCompletionSource<bool>();
            await Navigation.PushPopupAsync(this);

            return await _tcs.Task;
        }
    }
}
