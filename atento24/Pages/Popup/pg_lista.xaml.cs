using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_lista : PopupPage
    {
        TaskCompletionSource<string> _tcs = null;

        public pg_lista(List<ent_mensaje> lista)
        {
            InitializeComponent();
            AlturaList(lista.Count);
            lbl_titulo.Text = VarGlobal.tit_mensaje;
            lv_opcion.ItemsSource = lista;
        }

        private void AlturaList(int count)
        {
            if (count >= 3 || count <= 5)
            {
                switch (count)
                {
                    case 3:
                        lv_opcion.HeightRequest = 50 * 3;
                        break;
                    case 4:
                        lv_opcion.HeightRequest = 50 * 4;
                        break;
                    case 5:
                        lv_opcion.HeightRequest = 50 * 5;
                        break;
                }
            }
            if (count > 5)
            {
                lv_opcion.HeightRequest = 50 * 5;
            }
        }

        private async void Close_List(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void lv_opcion_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var entidad = ((ListView)sender).SelectedItem as ent_mensaje;
            _tcs?.SetResult(entidad.cod_mensaje);
            //_ent?.SetResult(entidad);
            await Navigation.PopAllPopupAsync();
        }

        public async Task<string> Show()
        {
            _tcs = new TaskCompletionSource<string>();
            await Navigation.PushPopupAsync(this);
            return await _tcs.Task;
        }

    }
}
