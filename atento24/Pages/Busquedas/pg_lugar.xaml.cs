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
    public partial class pg_lugar : ContentPage
    {
        public List<lc_cat_lugar> lista_cat_lugar;

        public pg_lugar()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            switch (VarGlobal.cod_modulo_ret)
            {
                case "IP":
                case "HL":
                    lb_titulo.Text = VarGlobal.pro_hallazgo.nom_tipoubicacion;
                    break;
                case "PG":
                    lb_titulo.Text = VarGlobal.pro_tarea.nom_tipoubicacion;
                    break;
                case "VE":
                    lb_titulo.Text = VarGlobal.pro_veoregistro.nom_tipoubicacion;
                    break;
                case "TR":
                    lb_titulo.Text = VarGlobal.pro_tarea.nom_tipoubicacion;
                    break;
                case "IN":
                    lb_titulo.Text = VarGlobal.pro_incidente.nom_tipoubicacion;
                    break;
            }
            CargarLugar();
        }

        private void CargarLugar()
        {
            lc_cat_lugar_Data o_Data = new lc_cat_lugar_Data();
            lista_cat_lugar = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa && x.cod_unidad == VarGlobal.cod_unidad).ToList();
            LugarListView.ItemsSource = lista_cat_lugar;
        }

        private void btnLugar_Clicked(object sender, EventArgs e)
        {
            var lista = lista_cat_lugar.Where(x => x.nom_lugar.ToUpper().Contains(edLugar.Text.Trim().ToUpper())).ToList();
            LugarListView.ItemsSource = lista;
        }

        private void LugarListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lc_cat_lugar entidad = ((ListView)sender).SelectedItem as lc_cat_lugar;
            switch (VarGlobal.cod_modulo_ret)
            {
                case "IP":
                case "HL":
                    VarGlobal.pro_hallazgo.cod_lugar = entidad.cod_lugar;
                    VarGlobal.pro_hallazgo.nom_lugar = entidad.nom_lugar;
                    Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
                case "PG":
                    VarGlobal.pro_tarea.cod_lugar = entidad.cod_lugar;
                    VarGlobal.pro_tarea.nom_lugar = entidad.nom_lugar;
                    Navigation.PushAsync(new pg_pro_tarea_mnt("B") { Title = VarGlobal.pro_tarea.ret_titulo });
                    break;
                case "VE":
                    VarGlobal.pro_veoregistro.cod_lugar = entidad.cod_lugar;
                    VarGlobal.pro_veoregistro.nom_lugar = entidad.nom_lugar;
                    if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("N") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    else
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("B") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    break;
                case "TR":
                    VarGlobal.pro_tarea.cod_lugar = entidad.cod_lugar;
                    VarGlobal.pro_tarea.nom_lugar = entidad.nom_lugar;
                    Navigation.PushAsync(new pg_pro_tarea_mnt("B") { Title = VarGlobal.pro_tarea.ret_titulo });
                    break;
                case "IN":
                    VarGlobal.pro_incidente.cod_lugar = entidad.cod_lugar;
                    VarGlobal.pro_incidente.nom_lugar = entidad.nom_lugar;
                    Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
                    break;
            }
        }

        private void btnsalir_Clicked(object sender, EventArgs e)
        {
            switch (VarGlobal.cod_modulo_ret)
            {
                case "IP":
                case "HL":
                    Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
                case "PG":
                    Navigation.PushAsync(new pg_pro_tarea_mnt("B") { Title = VarGlobal.pro_tarea.ret_titulo });
                    break;
                case "VE":
                    if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("N") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    else
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("B") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    break;
                case "TR":
                    Navigation.PushAsync(new pg_pro_tarea_mnt("B") { Title = VarGlobal.pro_tarea.ret_titulo });
                    break;
                case "IN":
                    Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
                    break;
            }
        }

        private void LugarListView_Refreshing(object sender, EventArgs e)
        {
            CargarLugar();
            LugarListView.IsRefreshing = false;
        }
    }
}
