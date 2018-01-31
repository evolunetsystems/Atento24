using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_hallazgo_qry : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();

        public pg_pro_hallazgo_qry()
        {
            InitializeComponent();
            BindingContext = this;
            NavigationPage.SetHasBackButton(this, false);
            ListarObservaciones();
            //if (VarGlobal.pro_hallazgo.cod_modulo == "OB")
            if (VarGlobal.pro_hallazgo.cod_modulo == "OB")
            {
                slCabecera.IsVisible = false;
                btnSalir.IsVisible = false;
            }
            //lbl_cod.Text = VarGlobal.pro_hallazgo.cod_referencia;

            lbl_cod.Text = VarGlobal.pro_hallazgo.cod_referencia;
            lbl_tip.Text = VarGlobal.tip_inspeccion_hall;
            lbl_tit.Text = VarGlobal.tit_inspeccion_hall;
        }

        private void ListarObservaciones()
        {
            lc_pro_hallazgo_Data o_Data = new lc_pro_hallazgo_Data();
            lc_pro_tarea_Data o_Tarea = new lc_pro_tarea_Data();
            List<lc_pro_hallazgo> lista = new List<lc_pro_hallazgo>();

            if (VarGlobal.pro_hallazgo.cod_modulo == "OB")
            {
                lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad
                                                            && x.cod_modulo == VarGlobal.pro_hallazgo.cod_modulo
                                                            && x.cod_estado == "01"
                                                            && x.cod_personal == VarGlobal.cod_personal
                                                            ).ToList();
            }
            else
            {
                lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad
                                                            && x.cod_modulo == VarGlobal.pro_hallazgo.cod_modulo
                                                            && x.cod_estado == "01"
                                                            && x.cod_referencia == VarGlobal.pro_hallazgo.cod_referencia
                                                            && x.cod_personal == VarGlobal.cod_personal
                                                            ).ToList();
            }
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

                var nivel = lista[i].nom_tblnivelriesgo;
                switch (nivel)
                {
                    case "ALTO":
                        lista[i].niv_color = "#FF0000";
                        break;
                    case "MEDIO":
                        lista[i].niv_color = "#FF8000";
                        break;
                    case "BAJO":
                        lista[i].niv_color = "#FFBF00";
                        break;
                }
                if (lista[i].sincronizado)
                {
                    lista[i].sincr_color = "#04B404";
                }
                else
                {
                    lista[i].sincr_color = "#DF0101";
                }

                if (lista[i].cod_modulo == "OB")
                {
                    lista[i].ver_clase = false;
                }
                else
                {
                    lista[i].ver_clase = true;
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
                                                            && x.cod_referencia == lista[i].cod_hallazgo).Count();
                lista[i].ver_tarea = lista[i].num_tarea > 0;
            }
            ObservacionlistView.ItemsSource = lista.OrderByDescending(x => x.cod_hallazgo).ToList();
        }

        private async void btnCrear_Clicked(object sender, EventArgs e)
        {
            VarGlobal.pro_hallazgo = ((Button)sender).CommandParameter as lc_pro_hallazgo;
            VarGlobal.cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo;

            VarGlobal.pro_tarea = new lc_pro_tarea()
            {
                cod_empresa = VarGlobal.pro_hallazgo.cod_empresa,
                cod_unidad = VarGlobal.pro_hallazgo.cod_unidad,
                cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo,
                cod_modulo = "HL",
                cod_estado = "01",
                cod_personal = VarGlobal.pro_hallazgo.cod_personal,
                cod_tipoubicacion = VarGlobal.pro_hallazgo.cod_tipoubicacion,
                nom_tipoubicacion = VarGlobal.pro_hallazgo.nom_tipoubicacion,
                cod_labor = VarGlobal.pro_hallazgo.cod_labor,
                nom_labor = VarGlobal.pro_hallazgo.nom_labor,
                cod_lugar = VarGlobal.pro_hallazgo.cod_lugar,
                nom_lugar = VarGlobal.pro_hallazgo.nom_lugar,
                cod_equipo = VarGlobal.pro_hallazgo.cod_equipo,
                nom_equipo = VarGlobal.pro_hallazgo.nom_equipo,
                cod_modulo_2do = VarGlobal.cod_modulo_2do,
                por_avance = 0,
                ver_opcion = "",
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip,
                estado = "A",
                comando = "INS"
            };
            //pkAccion.Focus();

            //Invocando al Popup de lista
            List<ent_mensaje> lista = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "TAREAS";
            lista.Add(new ent_mensaje { cod_mensaje = "V", ico_mensaje = "fa-list-alt", tex_mensaje = "Ver Tarea(s)" });
            lista.Add(new ent_mensaje { cod_mensaje = "N", ico_mensaje = "fa-file-o", tex_mensaje = "Nueva Tarea" });

            var popupAlert = new pg_lista(lista);
            var result = await popupAlert.Show();
            switch (result)
            {
                case "V":
                    VarGlobal.ret_tarea_padre = "pg_pro_hallazgo_qry";
                    VarGlobal.ret_titulo = "Tareas";
                    VarGlobal.ver_opcion = "";
                    await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_qry"));
                    break;
                case "N":
                    VarGlobal.ret_tarea_hijo = "pg_pro_hallazgo_qry";
                    VarGlobal.ret_titulo = "Nueva Tarea";
                    VarGlobal.pro_tarea.cod_tarea = "";
                    await Navigation.PushAsync(new pg_pro_tarea_mnt("N") { Title = VarGlobal.ret_titulo });
                    break;
            }
            //await Navigation.PushPopupAsync(popupAlert);
        }

        private async void btnMas_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_hallazgo = ((Button)sender).CommandParameter as lc_pro_hallazgo;
            CargarDetalles();

            VarGlobal.ret_hallazgo_hijo = "pg_pro_hallazgo_qry";
            await Navigation.PushAsync(new pg_pro_hallazgo_det() { Title = "Det. Hallazgo: " + VarGlobal.pro_hallazgo.cod_hallazgo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void Nuevo_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_hallazgo.titulo = VarGlobal.pro_hallazgo.cod_modulo == "OB" ? "Nueva Observación" : "Nuevo Hallazgo";
            VarGlobal.pro_hallazgo.cod_referencia = lbl_cod.Text;
            VarGlobal.ret_hallazgo_hijo = "pg_pro_hallazgo_qry";

            await Navigation.PushAsync(new pg_pro_hallazgo_mnt("N") { Title = VarGlobal.pro_hallazgo.titulo });
            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnModi_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);
            VarGlobal.pro_hallazgo = ((Button)sender).CommandParameter as lc_pro_hallazgo;
            CargarDetalles();


            VarGlobal.pro_hallazgo.titulo = VarGlobal.pro_hallazgo.cod_modulo == "OB" ? "Modificar Observación" : "Modificar Hallazgo";
            VarGlobal.ret_hallazgo_hijo = "pg_pro_hallazgo_qry";
            await Navigation.PushAsync(new pg_pro_hallazgo_mnt("M") { Title = VarGlobal.pro_hallazgo.titulo });
            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void CargarDetalles()
        {
            lc_pro_evidencia_Data o_Data_Evi = new lc_pro_evidencia_Data();
            VarGlobal.pro_hallazgo.lst_lc_pro_evidencia = o_Data_Evi.Listar().Where(x => x.cod_empresa == VarGlobal.pro_hallazgo.cod_empresa
                                                                                && x.cod_unidad == VarGlobal.pro_hallazgo.cod_unidad
                                                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo).ToList();
            lc_pro_coordenada_Data o_Data_Coo = new lc_pro_coordenada_Data();
            VarGlobal.pro_hallazgo.lst_lc_pro_coordenada = o_Data_Coo.Listar().Where(x => x.cod_empresa == VarGlobal.pro_hallazgo.cod_empresa
                                                                                && x.cod_unidad == VarGlobal.pro_hallazgo.cod_unidad
                                                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo).ToList();
            lc_pro_participante_Data o_Data_Par = new lc_pro_participante_Data();
            VarGlobal.pro_hallazgo.lst_lc_pro_participante = o_Data_Par.Listar().Where(x => x.cod_empresa == VarGlobal.pro_hallazgo.cod_empresa
                                                                                && x.cod_unidad == VarGlobal.pro_hallazgo.cod_unidad
                                                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo
                                                                                ).ToList();
            lc_pro_tarea_Data o_Data_Tar = new lc_pro_tarea_Data();
            VarGlobal.pro_hallazgo.lst_lc_pro_tarea = o_Data_Tar.Listar().Where(x => x.cod_empresa == VarGlobal.pro_hallazgo.cod_empresa
                                                                                && x.cod_unidad == VarGlobal.pro_hallazgo.cod_unidad
                                                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo).ToList();

        }

        private void btnSalir_Clicked(object sender, EventArgs e)
        {
            Retornar();
        }

        private async void Retornar()
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Task.Run(() =>
            {
                Task.Delay(100).Wait();
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushPopupAsync(loadingPage);
                });
            });

            string s_retorno = VarGlobal.ret_hallazgo_padre;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void ObservacionlistView_Refreshing(object sender, EventArgs e)
        {
            ListarObservaciones();
            ObservacionlistView.IsRefreshing = false;
        }

        private void TapGestureRecognizer_Tapped_nivel(object sender, EventArgs e)
        {
            IGestureRecognizer tgrIcon = ((Label)sender).GestureRecognizers[0];
            VarGlobal.pro_hallazgo = ((TapGestureRecognizer)tgrIcon).CommandParameter as lc_pro_hallazgo;

            var entidad = new ent_mensaje()
            {
                ico_mensaje = "fa-exclamation-circle",
                tit_mensaje = "Nivel de riesgo",
                tex_mensaje = VarGlobal.pro_hallazgo.nom_tblnivelriesgo
            };
            switch (VarGlobal.pro_hallazgo.nom_tblnivelriesgo)
            {
                case "ALTO":
                    entidad.col_mensaje = "#FF0000";
                    break;
                case "MEDIO":
                    entidad.col_mensaje = "#FF8000";
                    break;
                case "BAJO":
                    entidad.col_mensaje = "#FFBF00";
                    break;
            }

            VarGlobal._mensaje = new pg_mensaje(entidad);
            Navigation.PushPopupAsync(VarGlobal._mensaje);
        }

        private void TapGestureRecognizer_Tapped_sincro(object sender, EventArgs e)
        {
            IGestureRecognizer tgrIcon = ((Label)sender).GestureRecognizers[0];
            VarGlobal.pro_hallazgo = ((TapGestureRecognizer)tgrIcon).CommandParameter as lc_pro_hallazgo;

            var entidad = new ent_mensaje()
            {
                ico_mensaje = "fa-cloud-upload",
                tit_mensaje = "Sincronizado",
                tex_mensaje = VarGlobal.pro_hallazgo.sincronizado ? "Registro Sincronizado" : "Registro NO Sincronizado",
                col_mensaje = VarGlobal.pro_hallazgo.sincronizado ? "#04B404" : "#DF0101"
            };

            VarGlobal._mensaje = new pg_mensaje(entidad);
            Navigation.PushPopupAsync(VarGlobal._mensaje);
        }

        private async void btnEli_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;

            VarGlobal.pro_hallazgo = ((Button)sender).CommandParameter as lc_pro_hallazgo;

            if (VarGlobal.pro_hallazgo.por_avance == 0
                && VarGlobal.pro_hallazgo.cod_estado == "01"
                && VarGlobal.pro_hallazgo.usu_crea == VarGlobal.cod_usuario)
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
                    lc_pro_hallazgo_Data o_Data_Hall = new lc_pro_hallazgo_Data();
                    o_Data_Hall.EliminarUno(VarGlobal.pro_hallazgo, false);

                    //InsertarProElimina
                    if (VarGlobal.pro_hallazgo.cod_hallazgo.Length == 12)
                    {
                        lc_pro_elimina_Data o_Data_Eli = new lc_pro_elimina_Data();
                        o_Data_Eli.Insertar(new lc_pro_elimina()
                        {
                            cod_empresa = VarGlobal.cod_empresa,
                            cod_unidad = VarGlobal.cod_unidad,
                            cod_modulo = VarGlobal.pro_hallazgo.cod_modulo,
                            cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo,
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
                    tex_mensaje = "No se puede eliminar Hallazgo"
                });
                await Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            await Navigation.RemovePopupPageAsync(loadingPage);

            Content.IsEnabled = true;
        }
    }
}
