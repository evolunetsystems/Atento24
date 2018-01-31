using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Busquedas;
using atento24.Pages.Popup;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_veoregistro_qry : ContentPage
    {
        public pg_pro_veoregistro_qry()
        {
            InitializeComponent();
            BindingContext = this;
            ListarVeos();
        }

        private void ListarVeos()
        {
            lc_pro_veoregistro_Data o_Data = new lc_pro_veoregistro_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad).ToList();

            string s_col_base = "#BDBDBD";
            for (int i = 0; i < lista.Count; i++)
            {
                //  Barra de Avance
                decimal d_avance = lista[i].por_veoregistro * 100;
                lista[i].i_avance = Convert.ToInt32(d_avance);
                string s_col_marca = "#41C571";

                lista[i].s_color_00 = s_col_marca;
                lista[i].s_color_01 = lista[i].i_avance > 5 ? s_col_marca : s_col_base;
                lista[i].s_color_02 = lista[i].i_avance > 15 ? s_col_marca : s_col_base;
                lista[i].s_color_03 = lista[i].i_avance > 25 ? s_col_marca : s_col_base;
                lista[i].s_color_04 = lista[i].i_avance > 35 ? s_col_marca : s_col_base;
                lista[i].s_color_05 = lista[i].i_avance > 45 ? s_col_marca : s_col_base;
                lista[i].s_color_06 = lista[i].i_avance > 55 ? s_col_marca : s_col_base;
                lista[i].s_color_07 = lista[i].i_avance > 65 ? s_col_marca : s_col_base;
                lista[i].s_color_08 = lista[i].i_avance > 75 ? s_col_marca : s_col_base;
                lista[i].s_color_09 = lista[i].i_avance > 85 ? s_col_marca : s_col_base;
                lista[i].s_color_10 = lista[i].i_avance > 95 ? s_col_marca : s_col_base;

                if (lista[i].sincronizado)
                {
                    lista[i].sincr_color = "#04B404";
                }
                else
                {
                    lista[i].sincr_color = "#DF0101";
                }
                switch (lista[i].cod_tipoubicacion)
                {
                    case "E":
                        lista[i].nom_ubicacion = lista[i].nom_equipo;
                        break;
                    case "I":
                        lista[i].nom_ubicacion = lista[i].nom_labor;
                        break;
                    case "S":
                        lista[i].nom_ubicacion = lista[i].nom_lugar;
                        break;
                }
            }

            lvLista.ItemsSource = lista.OrderByDescending(x => x.cod_veoregistro).ToList();
        }

        private void lvLista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void btnNue_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);
            VarGlobal.pro_veoregistro = new lc_pro_veoregistro();

            VarGlobal.pro_veoregistro.titulo = "Nuevo V.E.O.";
            VarGlobal.pro_veoregistro.retorno = "pg_pro_veoregistro_qry";

            VarGlobal.pro_veoregistro.cod_veoregistro = "";
            VarGlobal.pro_veoregistro.nom_ubicacion = "";
            VarGlobal.pro_veoregistro.nom_labor = "";
            VarGlobal.pro_veoregistro.nom_lugar = "";
            VarGlobal.pro_veoregistro.nom_equipo = "";
            await Navigation.PushAsync(new pg_plantillaveo() { Title = "Seleccionar" });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnModi_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_veoregistro = ((Button)sender).CommandParameter as lc_pro_veoregistro;
            if (VarGlobal.pro_veoregistro.cer_veoregistro == "S")
            {
                await DisplayAlert("Validación", "El VEO no se puede modificar...", "Aceptar");
            }
            else
            {
                //AsignarVEO(o_Entidad);
                VarGlobal.pro_veoregistro.titulo = "Modificar V.E.O.";
                VarGlobal.pro_veoregistro.retorno = "pg_pro_veoregistro_qry";
                await Navigation.PushAsync(new pg_pro_veoregistro_mnt("M") { Title = VarGlobal.pro_veoregistro.titulo });
            }

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnElim_Clicked(object sender, EventArgs e)
        {
            lc_pro_veoregistro o_Entidad = ((Button)sender).CommandParameter as lc_pro_veoregistro;
            var answer = await DisplayAlert(o_Entidad.nom_veoplantilla, "¿Desea Eliminar VEO?", "Si", "No");
            if (answer)
            {
                EliminarVEO(o_Entidad);
            }
        }

        private async void EliminarVEO(lc_pro_veoregistro o_Entidad)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            if (o_Entidad.cer_veoregistro == "N")
            {
                lc_pro_veoregistro_Data o_Data_Inc = new lc_pro_veoregistro_Data();
                o_Data_Inc.EliminarUno(o_Entidad);

                //InsertarProElimina
                if (o_Entidad.cod_veoregistro.Length == 12)
                {
                    lc_pro_elimina_Data o_Data_Eli = new lc_pro_elimina_Data();
                    o_Data_Eli.Insertar(new lc_pro_elimina()
                    {
                        cod_empresa = VarGlobal.cod_empresa,
                        cod_unidad = VarGlobal.cod_unidad,
                        cod_modulo = VarGlobal.cod_modulo,
                        cod_referencia = o_Entidad.cod_veoregistro,
                        ip = VarGlobal.ip
                    });
                }
            }
            else
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "Error de validación",
                    tex_mensaje = "VEO se encuentra en estado cerrado..."
                });
                await Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            await Navigation.PushPopupAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void lvLista_Refreshing(object sender, EventArgs e)
        {
            ListarVeos();
            lvLista.IsRefreshing = false;
        }

        private void btnDet_Clicked(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_sincro(object sender, EventArgs e)
        {
            IGestureRecognizer tgrIcon = ((Label)sender).GestureRecognizers[0];
            VarGlobal.pro_veoregistro = ((TapGestureRecognizer)tgrIcon).CommandParameter as lc_pro_veoregistro;

            var entidad = new ent_mensaje()
            {
                ico_mensaje = "fa-cloud-upload",
                tit_mensaje = "Sincronizado",
                tex_mensaje = VarGlobal.pro_veoregistro.sincronizado ? "Registro Sincronizado" : "Registro NO Sincronizado",
                col_mensaje = VarGlobal.pro_veoregistro.sincronizado ? "#04B404" : "#DF0101"
            };

            VarGlobal._mensaje = new pg_mensaje(entidad);
            Navigation.PushPopupAsync(VarGlobal._mensaje);
        }
    }
}
