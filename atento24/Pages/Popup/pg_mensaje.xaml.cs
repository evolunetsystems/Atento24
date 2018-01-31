using System;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_mensaje : PopupPage
    {
        public pg_mensaje(ent_mensaje entidad)
        {
            InitializeComponent();
            switch (entidad.tip_mensaje)
            {
                case "INF":
                    entidad.ico_mensaje = "fa-info-circle";
                    entidad.col_mensaje = "#04B404";
                    break;
                case "WAR":
                    entidad.ico_mensaje = "fa-exclamation-triangle";
                    entidad.col_mensaje = "#FF8000";
                    break;
                case "ERR":
                    entidad.ico_mensaje = "fa-times-circle";
                    entidad.col_mensaje = "#FF0000";
                    break;
            }
            AsignarValores(entidad);
        }

        private void AsignarValores(ent_mensaje entidad)
        {
            sl_icon.BackgroundColor = Color.FromHex(entidad.col_mensaje);
            ilb_icono.Text = entidad.ico_mensaje;
            lb_titulo.Text = entidad.tit_mensaje;
            lb_mensaje.Text = entidad.tex_mensaje;
        }

        private void icon_si_Clicked(object sender, EventArgs e)
        {
            Navigation.RemovePopupPageAsync(VarGlobal._mensaje);
        }
    }
}
