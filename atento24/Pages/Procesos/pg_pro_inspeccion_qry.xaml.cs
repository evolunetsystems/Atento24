using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Busquedas;
using atento24.Pages.Popup;
using atento24.Recursos;
using Plugin.Iconize;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_inspeccion_qry : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public pg_pro_inspeccion_qry()
        {
            InitializeComponent();
            ListarInspecciones();
        }

        private void ListarInspecciones()
        {
            lc_pro_hallazgo_Data o_Hallazgo = new lc_pro_hallazgo_Data();
            lc_pro_inspeccion_Data o_Data = new lc_pro_inspeccion_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                && x.cod_unidad == VarGlobal.cod_unidad
                                                && x.cod_estado == "01"
                                                && x.cod_personal == VarGlobal.cod_personal).ToList();

            string s_col_base = "#BDBDBD";
            for (int i = 0; i < lista.Count; i++)
            {
                //  Barra de Avance
                lista[i].i_avance = Convert.ToInt32(lista[i].por_avance);
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

                //  Cantidad de Hallazgos
                lista[i].num_hallazgo = o_Hallazgo.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad
                                                            && x.cod_referencia == lista[i].cod_inspeccion).Count();
                lista[i].ver_hallazgo = lista[i].num_hallazgo > 0;
            }
            InspeccionlistView.ItemsSource = lista.OrderByDescending(x => x.cod_inspeccion).ToList();
        }

        private async void btnHall_Clicked(object sender, EventArgs e)
        {
            VarGlobal.pro_inspeccion = ((Button)sender).CommandParameter as lc_pro_inspeccion;
            VarGlobal.pro_hallazgo = new lc_pro_hallazgo();
            VarGlobal.pro_hallazgo.cod_modulo = "IP";
            VarGlobal.pro_hallazgo.cod_referencia = VarGlobal.pro_inspeccion.cod_inspeccion;
            VarGlobal.pro_hallazgo.cod_personal = VarGlobal.pro_inspeccion.cod_personal;
            VarGlobal.tit_inspeccion_hall = VarGlobal.pro_inspeccion.tit_inspeccion;
            VarGlobal.tip_inspeccion_hall = VarGlobal.pro_inspeccion.nom_inspecciontipo;
            //pkHallazgo.Focus();

            //Invocando al Popup de lista
            List<ent_mensaje> lista = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "HALLAZGOS";
            lista.Add(new ent_mensaje { cod_mensaje = "V", ico_mensaje = "fa-list-alt", tex_mensaje = "Ver Hallazgo(s)" });
            lista.Add(new ent_mensaje { cod_mensaje = "N", ico_mensaje = "fa-file-o", tex_mensaje = "Nuevo Hallazgo" });
            var popupAlert = new pg_lista(lista);
            var result = await popupAlert.Show();
            switch (result)
            {
                case "V":
                    VarGlobal.ret_hallazgo_padre = "pg_pro_inspeccion_qry";
                    await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_hallazgo_qry"));
                    break;
                case "N":
                    VarGlobal.ret_hallazgo_padre = "pg_pro_inspeccion_qry";
                    VarGlobal.pro_hallazgo.titulo = "Nuevo Hallazgo";
                    var modulo = VarGlobal.pro_hallazgo.cod_modulo;
                    await Navigation.PushAsync(new pg_pro_hallazgo_mnt("N") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
            }
        }

        private async void btnMas_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_inspeccion = ((Button)sender).CommandParameter as lc_pro_inspeccion;
            await Navigation.PushAsync(new pg_pro_inspeccion_det() { Title = "Inspección: " + VarGlobal.pro_inspeccion.cod_inspeccion });
            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void Nuevo_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_inspeccion = new lc_pro_inspeccion();
            VarGlobal.pro_inspeccion.titulo = "Nueva Inspección";
            VarGlobal.pro_inspeccion.cod_inspeccion = "";
            await Navigation.PushAsync(new pg_pro_inspeccion_mnt("N") { Title = VarGlobal.pro_inspeccion.titulo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnMod_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_inspeccion = ((Button)sender).CommandParameter as lc_pro_inspeccion;

            VarGlobal.pro_inspeccion.titulo = "Modificar Inspección";
            await Navigation.PushAsync(new pg_pro_inspeccion_mnt("M") { Title = VarGlobal.pro_inspeccion.titulo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnEli_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;

            VarGlobal.pro_inspeccion = ((Button)sender).CommandParameter as lc_pro_inspeccion;

            if (VarGlobal.pro_inspeccion.por_avance == 0
                && VarGlobal.pro_inspeccion.cod_estado == "01"
                && VarGlobal.pro_inspeccion.usu_crea == VarGlobal.cod_usuario)
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "WAR",
                    tit_mensaje = "Inspección",
                    tex_mensaje = "¿Desea Eliminar Registro?"
                });
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_inspeccion_Data o_Data_Inc = new lc_pro_inspeccion_Data();
                    o_Data_Inc.EliminarUno(VarGlobal.pro_inspeccion);

                    //InsertarProElimina
                    if (VarGlobal.pro_inspeccion.cod_inspeccion.Length == 12)
                    {
                        lc_pro_elimina_Data o_Data_Eli = new lc_pro_elimina_Data();
                        o_Data_Eli.Insertar(new lc_pro_elimina()
                        {
                            cod_empresa = VarGlobal.cod_empresa,
                            cod_unidad = VarGlobal.cod_unidad,
                            cod_modulo = VarGlobal.cod_modulo,
                            cod_referencia = VarGlobal.pro_inspeccion.cod_inspeccion,
                            ip = VarGlobal.ip
                        });
                    }
                }
            }
            else
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "Error de validación",
                    tex_mensaje = "No se puede eliminar Inspección"
                });
                await Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            await Navigation.RemovePopupPageAsync(loadingPage);

            Content.IsEnabled = true;
        }

        private void InspeccionlistView_Refreshing(object sender, EventArgs e)
        {
            ListarInspecciones();
            InspeccionlistView.IsRefreshing = false;
        }

        private void TapGestureRecognizer_Tapped_sincro(object sender, EventArgs e)
        {
            IGestureRecognizer tgrIcon = ((Label)sender).GestureRecognizers[0];
            VarGlobal.pro_inspeccion = ((TapGestureRecognizer)tgrIcon).CommandParameter as lc_pro_inspeccion;

            var entidad = new ent_mensaje()
            {
                ico_mensaje = "fa-cloud-upload",
                tit_mensaje = "Sincronizado",
                tex_mensaje = VarGlobal.pro_inspeccion.sincronizado ? "Registro Sincronizado" : "Registro NO Sincronizado",
                col_mensaje = VarGlobal.pro_inspeccion.sincronizado ? "#04B404" : "#DF0101"
            };

            VarGlobal._mensaje = new pg_mensaje(entidad);
            Navigation.PushPopupAsync(VarGlobal._mensaje);
        }
    }
}
