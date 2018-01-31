using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Procesos;
using atento24.Recursos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Busquedas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_ocurrencia : ContentPage
    {
        public static string s_cod;
        public List<lc_cat_ocurrencia> lista_cat_ocurrencia;
        public pg_ocurrencia(string codigo)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            lb_titulo.Text = VarGlobal.pro_hallazgo.nom_tblocurrenciatipo;
            s_cod = codigo;
            CargarOcurrencia(codigo);
        }

        private void CargarOcurrencia(string codigo)
        {
            lc_cat_ocurrencia_Data o_Data = new lc_cat_ocurrencia_Data();
            lista_cat_ocurrencia = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa).ToList();
            OcurrenciaListView.ItemsSource = lista_cat_ocurrencia;
        }

        private void OcurrenciaListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lc_cat_ocurrencia entidad = ((ListView)sender).SelectedItem as lc_cat_ocurrencia;
            VarGlobal.pro_hallazgo.cod_ocurrencia = entidad.cod_ocurrencia;
            VarGlobal.pro_hallazgo.nom_ocurrencia = entidad.nom_ocurrencia;
            Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
        }

        private void btnFiltro_Clicked(object sender, EventArgs e)
        {
            var lista = lista_cat_ocurrencia.Where(x => x.nom_ocurrencia.ToUpper().Contains(edFiltro.Text.Trim().ToUpper())).ToList();
            OcurrenciaListView.ItemsSource = lista;
        }

        private void btnsalir_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
        }

        private void OcurrenciaListView_Refreshing(object sender, EventArgs e)
        {
            CargarOcurrencia(s_cod);
            OcurrenciaListView.IsRefreshing = false;
        }
    }
}
