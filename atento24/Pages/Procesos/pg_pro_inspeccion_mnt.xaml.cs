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
    public partial class pg_pro_inspeccion_mnt : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public static string s_comando;
        public pg_pro_inspeccion_mnt(string comando)
        {
            InitializeComponent();
            OnBackButtonPressed();
            NavigationPage.SetHasBackButton(this, false);
            LoadComando(comando);
            MostrarParticipantes();
            CargarVariables();
        }

        private void LoadComando(string comando)
        {
            DateTime fecha = dpFecha.Date;

            switch (comando)
            {
                case "N":
                    //VarGlobal.pro_inspeccion.Limpiar();
                    VarGlobal.pro_inspeccion.cod_inspeccionprog = "000000000001";
                    VarGlobal.pro_inspeccion.fec_inspeccion = fecha.ToString("dd/MM/yyyy");
                    VarGlobal.pro_inspeccion.lst_lc_pro_participante = new List<lc_pro_participante>();
                    dpFechaLabel.Text = VarGlobal.pro_inspeccion.fec_inspeccion;
                    lbl_reportado.Text = VarGlobal.nom_personal;
                    lbl_cod_personal.Text = VarGlobal.cod_personal;
                    s_comando = "N";
                    break;
                case "B":
                    dpFechaLabel.Text = VarGlobal.pro_inspeccion.fec_inspeccion;
                    edTitulo.Text = VarGlobal.pro_inspeccion.tit_inspeccion;
                    edObjetivo.Text = VarGlobal.pro_inspeccion.obj_inspeccion;
                    pkSistemaLabel.Text = VarGlobal.pro_inspeccion.nom_sisgestion;
                    pkTipoLabel.Text = VarGlobal.pro_inspeccion.nom_inspecciontipo;
                    pkPreLabel.Text = VarGlobal.pro_inspeccion.nom_inspeccionpre;
                    if (string.IsNullOrEmpty(VarGlobal.pro_inspeccion.nom_personal))
                    {
                        lbl_reportado.Text = VarGlobal.nom_personal;
                        lbl_cod_personal.Text = VarGlobal.cod_personal;
                    }
                    else
                    {
                        lbl_reportado.Text = VarGlobal.pro_inspeccion.nom_personal;
                        lbl_cod_personal.Text = VarGlobal.pro_inspeccion.cod_personal;
                    }
                    break;
                case "M":
                    dpFechaLabel.Text = VarGlobal.pro_inspeccion.fec_inspeccion;
                    edTitulo.Text = VarGlobal.pro_inspeccion.tit_inspeccion;
                    edObjetivo.Text = VarGlobal.pro_inspeccion.obj_inspeccion;
                    pkSistemaLabel.Text = VarGlobal.pro_inspeccion.nom_sisgestion;
                    pkTipoLabel.Text = VarGlobal.pro_inspeccion.nom_inspecciontipo;
                    pkPreLabel.Text = VarGlobal.pro_inspeccion.nom_inspeccionpre;
                    lbl_cod_personal.Text = VarGlobal.pro_inspeccion.cod_personal;
                    lbl_reportado.Text = VarGlobal.pro_inspeccion.nom_personal;
                    CargarParticipantes();
                    s_comando = "M";
                    break;
            }
        }

        private void CargarVariables()
        {
            VarGlobal.pro_inspeccion.cod_empresa = VarGlobal.cod_empresa;
            VarGlobal.pro_inspeccion.cod_unidad = VarGlobal.cod_unidad;
            VarGlobal.pro_inspeccion.cod_usuario = VarGlobal.cod_usuario;
        }

        private void CargarParticipantes()
        {
            lc_pro_participante_Data o_Data = new lc_pro_participante_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_inspeccion.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_inspeccion.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_inspeccion.cod_inspeccion
                                                && x.tip_participante == "E").ToList();
            VarGlobal.pro_inspeccion.lst_lc_pro_participante = lista;
        }

        private void MostrarParticipantes()
        {
            List<lc_pro_participante> lista = VarGlobal.pro_inspeccion.lst_lc_pro_participante;
            if (lista != null)
            {
                //  Creando StackLayout Contenedor.
                for (int i = 0; i < lista.Count; i++)
                {
                    IconButton btnEli = new IconButton
                    {
                        WidthRequest = 40,
                        Text = "fa-times",
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 20,
                        TextColor = Color.FromHex("#B40404"),
                        CommandParameter = lista[i].cod_personal
                    };

                    btnEli.Clicked += btnEliParticipanteClicked;

                    StackLayout stCon = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Fill,
                        Margin = new Thickness(0, 0, 0, 1),
                        Padding = new Thickness(10, 0, 10, 0),
                        BackgroundColor = Color.White,
                        Children = {
                            new IconImage{
                                Icon="fa-user-circle",
                                IconColor = Color.Black,
                                VerticalOptions = LayoutOptions.Center,
                                IconSize = 20,
                                WidthRequest = 30,
                                HeightRequest = 30
                            },
                            new Label {
                                Text = lista[i].nom_personal,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.Center,
                                FontSize = 12
                            },

                        }
                    };
                    stCon.Children.Add(btnEli);
                    stInf.Children.Add(stCon);
                }
            }
        }

        #region Seleccionar Picker

        private async void pkSistemaLabel_Tapped(object sender, EventArgs e)
        {
            lc_acc_unidad_sisgestion_Data o_Data = new lc_acc_unidad_sisgestion_Data();
            var lst_sisgestion = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                           && x.cod_unidad == VarGlobal.cod_unidad).ToList();
            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "SISTEMAS";
            for (int i = 0; i < lst_sisgestion.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_sisgestion[i].cod_sisgestion,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_sisgestion[i].nom_sisgestion
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_inspeccion.cod_sisgestion = result;
                VarGlobal.pro_inspeccion.nom_sisgestion = lst_sisgestion.Where(x => x.cod_sisgestion == result).FirstOrDefault().nom_sisgestion.ToString();

                pkSistemaLabel.Text = VarGlobal.pro_inspeccion.nom_sisgestion;
            }
        }

        private async void pkTipoLabel_Tapped(object sender, EventArgs e)
        {
            lc_glb_inspecciontipo_Data o_Data = new lc_glb_inspecciontipo_Data();
            var lst_tipo = o_Data.Listar();

            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "TIPO";
            for (int i = 0; i < lst_tipo.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_tipo[i].cod_inspecciontipo,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_tipo[i].nom_inspecciontipo
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_inspeccion.cod_inspecciontipo = result;
                VarGlobal.pro_inspeccion.nom_inspecciontipo = lst_tipo.Where(x => x.cod_inspecciontipo == result).FirstOrDefault().nom_inspecciontipo.ToString();

                pkTipoLabel.Text = VarGlobal.pro_inspeccion.nom_inspecciontipo;
            }
        }

        private async void pkPreLabel_Tapped(object sender, EventArgs e)
        {
            lc_cat_inspeccionpre_Data o_Data = new lc_cat_inspeccionpre_Data();
            var lst_inspeccionpre = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa).ToList();

            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "PREDEFINIDA";
            for (int i = 0; i < lst_inspeccionpre.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_inspeccionpre[i].cod_inspeccionpre,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_inspeccionpre[i].nom_inspeccionpre
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_inspeccion.cod_inspeccionpre = result;
                VarGlobal.pro_inspeccion.nom_inspeccionpre = lst_inspeccionpre.Where(x => x.cod_inspeccionpre == result).FirstOrDefault().nom_inspeccionpre.ToString();

                pkPreLabel.Text = VarGlobal.pro_inspeccion.nom_inspeccionpre;
            }
        }
        #endregion

        #region Button

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "WAR",
                tit_mensaje = "Inspecciones",
                tex_mensaje = "¿Seguro que deseas salir?"
            });
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                Retornar();
            }
            await Navigation.RemovePopupPageAsync(loadingPage);

        }

        private async void Retornar()
        {
            await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_inspeccion_qry"));
        }

        private async void btn_grabar_Clicked(object sender, EventArgs e)
        {
            if (!ValidarInspeccion())
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "INF",
                    tit_mensaje = "Inspección",
                    tex_mensaje = "¿Desea Grabar Registro?"
                });
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_inspeccion_Data o_Data = new lc_pro_inspeccion_Data();

                    //  Generar Codigo, solo si viene VACIO O NULO
                    if (string.IsNullOrEmpty(VarGlobal.pro_inspeccion.cod_inspeccion))
                    {
                        var conteo = (o_Data.Listar().Count) + 1;
                        var año = VarGlobal.pro_inspeccion.fec_inspeccion.Substring(8, 2);
                        var mes = VarGlobal.pro_inspeccion.fec_inspeccion.Substring(3, 2);
                        string s_codigo = VarGlobal.cod_modulo + año + mes + "-" + conteo;

                        VarGlobal.pro_inspeccion.cod_inspeccion = s_codigo;
                        VarGlobal.pro_inspeccion.cod_personal = lbl_cod_personal.Text;
                        VarGlobal.pro_inspeccion.nom_personal = lbl_reportado.Text;
                        VarGlobal.pro_inspeccion.cod_estado = "01";

                    }
                    VarGlobal.pro_inspeccion.sincronizado = false;
                    o_Data.Modificar(VarGlobal.pro_inspeccion);
                    Retornar();
                }
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        private void btn_reportado_Clicked(object sender, EventArgs e)
        {
            var btn = ((Button)sender).CommandParameter;
            switch (btn)
            {
                case "Rep":
                    Navigation.PushAsync(new pg_personal("IP", "Rep") { Title = "Seleccionar" });
                    break;
                case "Inf":
                    Navigation.PushAsync(new pg_personal("IP", "Inf") { Title = "Seleccionar" });
                    break;
                case "Eva":
                    Navigation.PushAsync(new pg_personal("IP", "Eva") { Title = "Seleccionar" });
                    break;
            }
        }

        void btnEliParticipanteClicked(object sender, EventArgs e)
        {
            var s_cod_personal = ((Button)sender).CommandParameter;
            var lista = VarGlobal.pro_inspeccion.lst_lc_pro_participante;
            for (int i = 0; i < lista.Count; i++)
            {
                string s_cod = lista[i].cod_personal;
                if (s_cod == s_cod_personal.ToString())
                {
                    lista.RemoveAt(i);
                    i--;
                }
            }
            VarGlobal.pro_inspeccion.lst_lc_pro_participante = lista;
            stInf.Children.Remove(stInf);
            Navigation.PushAsync(new pg_pro_inspeccion_mnt("B") { Title = VarGlobal.pro_inspeccion.titulo });
        }

        #endregion

        private bool ValidarInspeccion()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(pkTipoLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Tipo...";
            }

            if (string.IsNullOrEmpty(edTitulo.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Título...";
            }

            if (string.IsNullOrEmpty(edObjetivo.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Objetivo...";
            }

            if (string.IsNullOrEmpty(pkPreLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Insp. Predefinida...";
            }

            if (string.IsNullOrEmpty(pkSistemaLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Sistema...";
            }

            if (string.IsNullOrEmpty(lbl_reportado.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Reportante...";
            }

            if (b_Error)
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "Error de validación",
                    tex_mensaje = s_Mensaje
                });
                Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            return b_Error;
        }

        private void dpFecha_DateSelected(object sender, DateChangedEventArgs e)
        {
            var fecha = ((DatePicker)sender).Date;
            dpFechaLabel.Text = fecha.ToString("dd/MM/yyyy");
        }

        private void dpFechaLabel_Tapped(object sender, EventArgs e)
        {
            dpFecha.Focus();
        }

        #region Editor

        private void edTitulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            VarGlobal.pro_inspeccion.tit_inspeccion = texto;
        }

        private void edObjetivo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            VarGlobal.pro_inspeccion.obj_inspeccion = texto;
        }

        #endregion

    }
}
