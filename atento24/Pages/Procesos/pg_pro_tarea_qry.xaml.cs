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
    public partial class pg_pro_tarea_qry : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public pg_pro_tarea_qry()
        {
            InitializeComponent();
            BindingContext = this;
            IsBusy = false;
            NavigationPage.SetHasBackButton(this, false);

            ListarTareas();
            //if (VarGlobal.comentar == 1 || VarGlobal.cod_modulo == "")
            if (VarGlobal.cod_modulo == "")
            {
                slCabecera.IsVisible = false;
                btnNue.IsVisible = false;
            }
            else
            {
                switch (VarGlobal.cod_modulo)
                {
                    case "HL":
                    case "OB":
                        lbl_cod.Text = VarGlobal.cod_referencia;
                        lbl_Cla.Text = VarGlobal.pro_hallazgo.nom_hallazgoclase;
                        lbl_Des.Text = VarGlobal.pro_hallazgo.des_hallazgo;
                        break;
                    case "IN":
                        lbl_cod.Text = VarGlobal.cod_referencia;
                        lbl_Cla_Tit.Text = "Sistema";
                        lbl_Cla.Text = VarGlobal.pro_incidente.nom_sisgestion;
                        lbl_Des_Tit.Text = "Descripción";
                        lbl_Des.Text = VarGlobal.pro_incidente.des_incidente;
                        break;
                    case "IP":
                        lbl_cod.Text = VarGlobal.cod_referencia;
                        lbl_Cla.Text = VarGlobal.pro_hallazgo.nom_hallazgoclase;
                        lbl_Des.Text = VarGlobal.pro_hallazgo.des_hallazgo;
                        break;
                }

            }
        }

        private void ListarTareas()
        {
            lc_pro_avance_Data o_Avance = new lc_pro_avance_Data();
            lc_pro_tarea_Data o_Data = new lc_pro_tarea_Data();
            List<lc_pro_tarea> lista = o_Data.Listar();

            if (VarGlobal.cod_modulo == "")
            {
                switch (VarGlobal.ver_opcion)
                {
                    case "A":
                        lista = lista.Where(x => x.eje_personal == VarGlobal.cod_personal && x.ver_opcion.Contains("A")).ToList();
                        break;
                    case "V":
                        lista = lista.Where(x => x.sol_personal == VarGlobal.cod_personal && x.ver_opcion.Contains("V")).ToList();
                        break;
                    case "X":
                        lista = lista.Where(x => x.sol_personal != VarGlobal.cod_personal
                                         && x.eje_personal != VarGlobal.cod_personal
                                         && x.ver_opcion.Contains("X")).ToList();
                        break;
                    case "S":
                        lista = lista.Where(x => x.usu_crea == VarGlobal.cod_usuario).ToList();
                        break;
                }
            }
            else
            {
                lista = lista.Where(x => x.cod_empresa == VarGlobal.cod_empresa
                        && x.cod_unidad == VarGlobal.cod_unidad
                        && x.cod_referencia == VarGlobal.cod_referencia).ToList();
            }


            string s_col_base = "#BDBDBD";
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].fecha = lista[i].ini_tarea + " - " + lista[i].fin_tarea;

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

                lista[i].coment_boton = (lista[i].cod_tarea.Trim().Length == 12);

                switch (lista[i].cod_estado)
                {
                    case "01":
                    case "03":
                        lista[i].btn_opcion = "fa-rocket";
                        lista[i].ena_boton = true;
                        break;
                    case "02":
                        lista[i].btn_opcion = "fa-gavel";
                        lista[i].ena_boton = true;
                        break;
                    case "04":
                        lista[i].ena_boton = false;
                        lista[i].coment_boton = false;
                        break;
                }
                //  opción para mostrar el botón de atender o verificar                
                lista[i].ver_btnAtender = (VarGlobal.ver_opcion == "A" || VarGlobal.ver_opcion == "V");


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

                //  Cantidad de Comentarios
                lista[i].num_comentario = o_Avance.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad
                                                            && x.cod_referencia == lista[i].cod_tarea).Count();
                lista[i].ver_comentario = lista[i].num_comentario > 0;
            }
            AccionlistView.ItemsSource = lista;
        }

        private async void btnAtender_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_tarea = ((Button)sender).CommandParameter as lc_pro_tarea;


            VarGlobal.ret_tarea_hijo = "pg_pro_tarea_qry";
            switch (VarGlobal.ver_opcion)
            {
                case "A":
                    VarGlobal.tip_etapa = "A";
                    if (VarGlobal.pro_tarea.cod_tarea.Length == 12
                        && (VarGlobal.pro_tarea.cod_estado == "01" || VarGlobal.pro_tarea.cod_estado == "04"))
                    {
                        await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_ate"));
                    }
                    else
                    {
                        VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                        {
                            tip_mensaje = "ERR",
                            tit_mensaje = "Atender Tarea",
                            tex_mensaje = "Opción Bloqueada."
                        });
                        await Navigation.PushPopupAsync(VarGlobal._mensaje);
                    }
                    break;
                case "V":
                    VarGlobal.tip_etapa = "E";
                    if (VarGlobal.pro_tarea.cod_tarea.Length == 12 && VarGlobal.pro_tarea.cod_estado == "02")
                    {
                        await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_ver"));
                    }
                    else
                    {
                        VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                        {
                            tip_mensaje = "ERR",
                            tit_mensaje = "Verificar Tarea",
                            tex_mensaje = "Opción Bloqueada."
                        });
                        await Navigation.PushPopupAsync(VarGlobal._mensaje);
                    }
                    break;
            }



            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnComentar_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_tarea = ((Button)sender).CommandParameter as lc_pro_tarea;

            if (VarGlobal.pro_tarea.cod_estado != "04")
            {
                VarGlobal.ret_tarea_hijo = "pg_pro_tarea_qry";
                VarGlobal.tip_etapa = "A";
                await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_com"));
            }
            else
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "No se puede Comentar",
                    tex_mensaje = "Registro no sincronizado"
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

            VarGlobal.pro_tarea = ((Button)sender).CommandParameter as lc_pro_tarea;
            //VarGlobal.pro_tarea.ret_hijo = "pg_pro_tarea_qry";
            VarGlobal.ret_tarea_hijo = "pg_pro_tarea_qry";
            await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_mas"));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void CargarLogin()
        {
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);
        }

        private async void btnsalir_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            //string s_retorno = VarGlobal.pro_tarea.ret_padre;
            string s_retorno = VarGlobal.ret_tarea_padre;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnNue_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);



            VarGlobal.pro_tarea.cod_tarea = "";
            VarGlobal.pro_tarea.des_origen = "Tarea";
            VarGlobal.pro_tarea.ret_titulo = "Nueva Tarea";
            VarGlobal.ret_tarea_hijo = "pg_pro_tarea_qry";
            await Navigation.PushAsync(new pg_pro_tarea_mnt("N") { Title = VarGlobal.pro_tarea.ret_titulo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async void btnModi_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_tarea = ((Button)sender).CommandParameter as lc_pro_tarea;
            if (VarGlobal.pro_tarea.cod_estado == "01"
                && VarGlobal.pro_tarea.usu_crea == VarGlobal.cod_usuario)
            {
                VarGlobal.ret_tarea_hijo = "pg_pro_tarea_qry";
                VarGlobal.pro_tarea.ret_titulo = "Modificar Tarea";
                await Navigation.PushAsync(new pg_pro_tarea_mnt("M") { Title = VarGlobal.pro_tarea.ret_titulo });
            }
            else
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "No se puede modificar",
                    tex_mensaje = "Tarea presenta un avance del " + VarGlobal.pro_tarea.por_avance.ToString("###").Trim() + " %"
                });
                await Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void AccionlistView_Refreshing(object sender, EventArgs e)
        {
            ListarTareas();
            AccionlistView.IsRefreshing = false;
        }

        private void CargarEvidencias()
        {
            lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
            var lst_entidad = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                 && x.cod_unidad == VarGlobal.cod_unidad
                                                 && x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).ToList();
            VarGlobal.pro_tarea.lst_lc_pro_evidencia = lst_entidad;
        }

        private async void btnEli_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;

            VarGlobal.pro_tarea = ((Button)sender).CommandParameter as lc_pro_tarea;

            if (VarGlobal.pro_tarea.por_avance == 0
                && VarGlobal.pro_tarea.cod_estado == "01"
                && VarGlobal.pro_tarea.usu_crea == VarGlobal.cod_usuario || VarGlobal.cod_perfil == "SA")
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "WAR",
                    tit_mensaje = "Tárea",
                    tex_mensaje = "¿Desea Eliminar Registro?"
                });
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_tarea_Data o_Data = new lc_pro_tarea_Data();
                    o_Data.EliminarUno(VarGlobal.pro_tarea, true);

                    //InsertarProElimina
                    if (VarGlobal.pro_tarea.cod_tarea.Length == 12)
                    {
                        lc_pro_elimina_Data o_Data_Eli = new lc_pro_elimina_Data();
                        o_Data_Eli.Insertar(new lc_pro_elimina()
                        {
                            cod_empresa = VarGlobal.pro_tarea.cod_empresa,
                            cod_unidad = VarGlobal.pro_tarea.cod_unidad,
                            cod_modulo = VarGlobal.pro_tarea.cod_modulo,
                            cod_referencia = VarGlobal.pro_tarea.cod_tarea,
                            ip = VarGlobal.ip
                        });
                    }

                    //ListarTareas
                    ListarTareas();
                }
            }
            else
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "Error de Validación",
                    tex_mensaje = "No se puede eliminar Tarea"
                });
                await Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void TapGestureRecognizer_Tapped_sincro(object sender, EventArgs e)
        {
            IGestureRecognizer tgrIcon = ((Label)sender).GestureRecognizers[0];
            VarGlobal.pro_tarea = ((TapGestureRecognizer)tgrIcon).CommandParameter as lc_pro_tarea;

            var entidad = new ent_mensaje()
            {
                ico_mensaje = "fa-cloud-upload",
                tit_mensaje = "Sincronizado",
                tex_mensaje = VarGlobal.pro_tarea.sincronizado ? "Registro Sincronizado" : "Registro NO Sincronizado",
                col_mensaje = VarGlobal.pro_tarea.sincronizado ? "#04B404" : "#DF0101"
            };

            VarGlobal._mensaje = new pg_mensaje(entidad);
            Navigation.PushPopupAsync(VarGlobal._mensaje);
        }
    }
}
