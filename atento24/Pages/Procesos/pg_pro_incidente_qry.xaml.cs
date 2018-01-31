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
    public partial class pg_pro_incidente_qry : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public pg_pro_incidente_qry()
        {
            InitializeComponent();
            ListarIncidentes();
        }

        private void ListarIncidentes()
        {
            lc_pro_tarea_Data o_Tarea = new lc_pro_tarea_Data();
            lc_pro_incidente_Data o_Data = new lc_pro_incidente_Data();
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

                //  Cantidad de Tareas
                lista[i].num_tarea = o_Tarea.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad
                                                            && x.cod_referencia == lista[i].cod_incidente).Count();
                lista[i].ver_tarea = lista[i].num_tarea > 0;
            }
            IncidentelistView.ItemsSource = lista.OrderByDescending(x => x.cod_incidente).ToList();
        }

        private async void btnCrear_Clicked(object sender, EventArgs e)
        {
            VarGlobal.pro_incidente = ((Button)sender).CommandParameter as lc_pro_incidente;
            VarGlobal.cod_referencia = VarGlobal.pro_incidente.cod_incidente;

            VarGlobal.pro_tarea = new lc_pro_tarea()
            {
                cod_empresa = VarGlobal.pro_incidente.cod_empresa,
                cod_unidad = VarGlobal.pro_incidente.cod_unidad,
                cod_referencia = VarGlobal.pro_incidente.cod_incidente,
                cod_modulo = "IN",
                cod_estado = "01",
                cod_personal = VarGlobal.pro_incidente.cod_personal,
                cod_tipoubicacion = VarGlobal.pro_incidente.cod_tipoubicacion,
                nom_tipoubicacion = VarGlobal.pro_incidente.nom_tipoubicacion,
                cod_labor = VarGlobal.pro_incidente.cod_labor,
                nom_labor = VarGlobal.pro_incidente.nom_labor,
                cod_lugar = VarGlobal.pro_incidente.cod_lugar,
                nom_lugar = VarGlobal.pro_incidente.nom_lugar,
                cod_equipo = VarGlobal.pro_incidente.cod_equipo,
                nom_equipo = VarGlobal.pro_incidente.nom_equipo,
                cod_modulo_2do = VarGlobal.cod_modulo_2do,
                por_avance = 0,
                ver_opcion = "",
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip,
                estado = "A",
                comando = "INS"
            };

            //Invocando al Popup de lista
            List<ent_mensaje> lista = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "TAREAS";
            lista.Add(new ent_mensaje { cod_mensaje = "V", ico_mensaje = "fa-file-text-o", tex_mensaje = "Ver Tarea(s)" });
            lista.Add(new ent_mensaje { cod_mensaje = "N", ico_mensaje = "fa-file-o", tex_mensaje = "Nueva Tarea" });

            var popupAlert = new pg_lista(lista);
            var result = await popupAlert.Show();
            switch (result)
            {
                case "V":
                    VarGlobal.ret_tarea_padre = "pg_pro_incidente_qry";
                    VarGlobal.ret_titulo = "Tareas";
                    VarGlobal.ver_opcion = "";
                    await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_qry"));
                    break;
                case "N":
                    VarGlobal.ret_tarea_hijo = "pg_pro_incidente_qry";
                    VarGlobal.ret_titulo = "Nueva Tarea";
                    VarGlobal.pro_tarea.cod_tarea = "";
                    await Navigation.PushAsync(new pg_pro_tarea_mnt("N") { Title = VarGlobal.ret_titulo });
                    break;
            }
        }

        private async void btnEli_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;

            VarGlobal.pro_incidente = ((Button)sender).CommandParameter as lc_pro_incidente;

            if (VarGlobal.pro_incidente.por_avance == 0
                && VarGlobal.pro_incidente.cod_estado == "01"
                && VarGlobal.pro_incidente.usu_crea == VarGlobal.cod_usuario)
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "WAR",
                    tit_mensaje = "Hallazgo",
                    tex_mensaje = "¿Desea Eliminar Registro?"
                });
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_incidente_Data o_Data_Inc = new lc_pro_incidente_Data();
                    o_Data_Inc.EliminarUno(VarGlobal.pro_incidente, false);

                    //InsertarProElimina
                    if (VarGlobal.pro_incidente.cod_incidente.Length == 12)
                    {
                        lc_pro_elimina_Data o_Data_Eli = new lc_pro_elimina_Data();
                        o_Data_Eli.Insertar(new lc_pro_elimina()
                        {
                            cod_empresa = VarGlobal.cod_empresa,
                            cod_unidad = VarGlobal.cod_unidad,
                            cod_modulo = VarGlobal.cod_modulo,
                            cod_referencia = VarGlobal.pro_incidente.cod_incidente,
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
                    tex_mensaje = "No se puede eliminar Incidente"
                });
                await Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            await Navigation.RemovePopupPageAsync(loadingPage);

            Content.IsEnabled = true;
        }

        private async void btnMas_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_incidente = ((Button)sender).CommandParameter as lc_pro_incidente;
            VarGlobal.pro_incidente.retorno = "pg_pro_incidente_qry";
            await Navigation.PushAsync(new pg_pro_incidente_det() { Title = "Incidente: " + VarGlobal.pro_incidente.cod_incidente });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void Nuevo_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_incidente = new lc_pro_incidente();
            VarGlobal.pro_incidente.titulo = "Nuevo Accidente";
            VarGlobal.pro_incidente.cod_incidente = "";
            VarGlobal.pro_incidente.retorno = "pg_pro_incidente_qry";
            await Navigation.PushAsync(new pg_pro_incidente_mnt("N") { Title = VarGlobal.pro_incidente.titulo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnModi_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_incidente = ((Button)sender).CommandParameter as lc_pro_incidente;

            VarGlobal.pro_incidente.titulo = "Modificar Accidente";
            VarGlobal.pro_incidente.retorno = "pg_pro_incidente_qry";
            await Navigation.PushAsync(new pg_pro_incidente_mnt("M") { Title = VarGlobal.pro_incidente.titulo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void IncidentelistView_Refreshing(object sender, EventArgs e)
        {
            ListarIncidentes();
            IncidentelistView.IsRefreshing = false;
        }

        private void TapGestureRecognizer_Tapped_sincro(object sender, EventArgs e)
        {
            IGestureRecognizer tgrIcon = ((Label)sender).GestureRecognizers[0];
            VarGlobal.pro_incidente = ((TapGestureRecognizer)tgrIcon).CommandParameter as lc_pro_incidente;

            var entidad = new ent_mensaje()
            {
                ico_mensaje = "fa-cloud-upload",
                tit_mensaje = "Sincronizado",
                tex_mensaje = VarGlobal.pro_incidente.sincronizado ? "Registro Sincronizado" : "Registro NO Sincronizado",
                col_mensaje = VarGlobal.pro_incidente.sincronizado ? "#04B404" : "#DF0101"
            };

            VarGlobal._mensaje = new pg_mensaje(entidad);
            Navigation.PushPopupAsync(VarGlobal._mensaje);
        }
    }
}
