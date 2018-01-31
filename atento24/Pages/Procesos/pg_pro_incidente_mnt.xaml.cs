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
    public partial class pg_pro_incidente_mnt : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public static string ubicacion;
        public static List<lc_acc_unidad_severidadreal> lst_real = new List<lc_acc_unidad_severidadreal>();
        public static List<lc_acc_unidad_severidadpot> lst_potencial = new List<lc_acc_unidad_severidadpot>();

        public pg_pro_incidente_mnt(string comando)
        {
            InitializeComponent();
            OnBackButtonPressed();
            NavigationPage.SetHasBackButton(this, false);
            LoadComando(comando);

            if (string.IsNullOrEmpty(pkSistemaLabel.Text))
            {
                //
                pkRealLabel.IsEnabled = false;
                btnReal.IsEnabled = false;
                pkPoteLabel.IsEnabled = false;
                btnPote.IsEnabled = false;
            }
            else
            {
                pkRealLabel.IsEnabled = true;
                btnReal.IsEnabled = true;
                pkPoteLabel.IsEnabled = true;
                btnPote.IsEnabled = true;
                CargarSevReal(VarGlobal.pro_incidente.cod_sisgestion);
                CargarSevPote(VarGlobal.pro_incidente.cod_sisgestion);
            }
            MostrarEvidencias();
            MostrarListaInfractores();
        }

        private void LoadComando(string comando)
        {
            DateTime fecha = dpFecha.Date;
            var hora = DateTime.Now.TimeOfDay.ToString();

            switch (comando)
            {
                case "N":
                    VarGlobal.pro_incidente.cod_empresa = VarGlobal.cod_empresa;
                    VarGlobal.pro_incidente.cod_unidad = VarGlobal.cod_unidad;
                    VarGlobal.pro_incidente.fec_incidente = (fecha.ToString("dd/MM/yyyy") + " " + hora).Substring(0, 19);
                    VarGlobal.pro_incidente.lst_lc_pro_incidente_personal = new List<lc_pro_incidente_personal>();
                    VarGlobal.pro_incidente.lst_lc_pro_evidencia = new List<lc_pro_evidencia>();
                    dpFechaLabel.Text = fecha.ToString("dd/MM/yyyy");
                    pkHoraLabel.Text = hora.Substring(0, 2);
                    pkMinutoLabel.Text = hora.Substring(3, 2);
                    lbl_reportado.Text = VarGlobal.nom_personal;
                    lbl_cod_personal.Text = VarGlobal.cod_personal;

                    break;
                case "B":
                    dpFechaLabel.Text = fecha.ToString("dd/MM/yyyy");
                    pkHoraLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(11, 2);
                    pkMinutoLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(14, 2);
                    pkSistemaLabel.Text = VarGlobal.pro_incidente.nom_sisgestion;
                    pkRealLabel.Text = VarGlobal.pro_incidente.nom_severidadreal;
                    pkPoteLabel.Text = VarGlobal.pro_incidente.nom_severidadpot;
                    pkUbicacionLabel.Text = VarGlobal.pro_incidente.nom_tipoubicacion;
                    lbl_des_incidente.Text = VarGlobal.pro_incidente.des_incidente;
                    if (string.IsNullOrEmpty(VarGlobal.pro_incidente.nom_personal))
                    {
                        lbl_reportado.Text = VarGlobal.nom_personal;
                        lbl_cod_personal.Text = VarGlobal.cod_personal;
                    }
                    else
                    {
                        lbl_reportado.Text = VarGlobal.pro_incidente.nom_personal;
                        lbl_cod_personal.Text = VarGlobal.pro_incidente.cod_personal;
                    }
                    MostrarUbicacion();
                    break;
                case "M":
                    dpFechaLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(0, 10);
                    pkHoraLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(11, 2);
                    pkMinutoLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(14, 2);
                    pkSistemaLabel.Text = VarGlobal.pro_incidente.nom_sisgestion;
                    pkRealLabel.Text = VarGlobal.pro_incidente.nom_severidadreal;
                    pkPoteLabel.Text = VarGlobal.pro_incidente.nom_severidadpot;
                    lbl_cod_personal.Text = VarGlobal.pro_incidente.cod_personal;
                    lbl_reportado.Text = VarGlobal.pro_incidente.nom_personal;
                    pkUbicacionLabel.Text = VarGlobal.pro_incidente.nom_tipoubicacion;
                    lbl_des_incidente.Text = VarGlobal.pro_incidente.des_incidente;
                    CargarPersonal();
                    CargarEvidencias();
                    MostrarUbicacion();
                    break;
            }
        }

        private void MostrarListaInfractores()
        {
            List<lc_pro_incidente_personal> lista = VarGlobal.pro_incidente.lst_lc_pro_incidente_personal;
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

                    btnEli.Clicked += btnEliInfractorClicked;

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

        async void btnEliInfractorClicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "INF",
                tit_mensaje = "Infractores",
                tex_mensaje = "¿Desea Eliminar Infractor?"
            });
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                var s_cod_personal = ((Button)sender).CommandParameter;
                var lista = VarGlobal.pro_incidente.lst_lc_pro_incidente_personal;
                for (int i = 0; i < lista.Count; i++)
                {
                    string s_cod = lista[i].cod_personal;
                    if (s_cod == s_cod_personal.ToString())
                    {
                        lista.RemoveAt(i);
                        i--;
                    }
                }
                VarGlobal.pro_incidente.lst_lc_pro_incidente_personal = lista;
                stInf.Children.Remove(stInf);
                await Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
            }
            await Navigation.RemovePopupPageAsync(loadingPage);
        }

        private void MostrarEvidencias()
        {
            List<lc_pro_evidencia> lista = VarGlobal.pro_incidente.lst_lc_pro_evidencia;
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
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        FontSize = 20,
                        TextColor = Color.FromHex("#B40404"),
                        CommandParameter = lista[i].num_evidencia
                    };

                    btnEli.Clicked += btnEliImagenClicked;

                    Stream stream = new MemoryStream(lista[i].dat_evidencia);
                    Image imgFoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return stream; }),
                        WidthRequest = 150,
                        HeightRequest = 150
                    };

                    Label lblCom = new Label
                    {
                        Text = lista[i].com_evidencia,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        FontSize = 12
                    };

                    StackLayout stCab = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Fill,
                        Margin = new Thickness(0, 0, 0, 1),
                        Padding = new Thickness(10, 10, 10, 10),
                        BackgroundColor = Color.White
                    };

                    StackLayout stCom = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(0, 0, 0, 1),
                        BackgroundColor = Color.White,
                        Spacing = 0
                    };

                    stCom.Children.Add(btnEli);
                    stCom.Children.Add(lblCom);
                    stCab.Children.Add(imgFoto);
                    stCab.Children.Add(stCom);
                    stDet.Children.Add(stCab);
                }
            }
        }

        async void btnEliImagenClicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "INF",
                tit_mensaje = "Evidencias",
                tex_mensaje = "¿Desea Eliminar Evidencia?"
            });
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                var s_cod_referencia = ((Button)sender).CommandParameter;
                var lista = VarGlobal.pro_incidente.lst_lc_pro_evidencia;
                for (int i = 0; i < lista.Count; i++)
                {
                    int s_cod = lista[i].num_evidencia;
                    if (s_cod == Convert.ToInt32(s_cod_referencia))
                    {
                        lista.RemoveAt(i);
                        i--;
                    }
                }
                VarGlobal.pro_incidente.lst_lc_pro_evidencia = lista;
                stInf.Children.Remove(stInf);
                await Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
            }
            await Navigation.RemovePopupPageAsync(loadingPage);
        }

        private async void btn_grabar_Clicked(object sender, EventArgs e)
        {
            if (!ValidarIncidente())
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "INF",
                    tit_mensaje = "Incidente",
                    tex_mensaje = "¿Desea Grabar Registro?"
                });
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_incidente_Data o_Data = new lc_pro_incidente_Data();
                    //  Generar Codigo, solo si viene VACIO O NULO
                    if (string.IsNullOrEmpty(VarGlobal.pro_incidente.cod_incidente))
                    {
                        var conteo = (o_Data.Listar().Count) + 1;
                        var año = VarGlobal.pro_incidente.fec_incidente.Substring(8, 2);
                        var mes = VarGlobal.pro_incidente.fec_incidente.Substring(3, 2);
                        string s_codigo = VarGlobal.cod_modulo + año + mes + "-" + conteo;

                        VarGlobal.pro_incidente.cod_incidente = s_codigo;
                        VarGlobal.pro_incidente.cod_personal = lbl_cod_personal.Text;
                        VarGlobal.pro_incidente.nom_personal = lbl_reportado.Text;
                        VarGlobal.pro_incidente.cod_estado = "01";
                        VarGlobal.pro_incidente.usu_crea = VarGlobal.cod_usuario;

                    }
                    VarGlobal.pro_incidente.sincronizado = false;
                    o_Data.Modificar(VarGlobal.pro_incidente);
                    await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_incidente_qry"));
                }
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        private bool ValidarIncidente()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(pkRealLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Sev Real...";
            }

            if (string.IsNullOrEmpty(pkPoteLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Sev Potencial...";
            }

            if (string.IsNullOrEmpty(pkSistemaLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Sistema...";
            }

            if (string.IsNullOrEmpty(lbl_des_incidente.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Descripción...";
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

        private void CargarEvidencias()
        {
            lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_incidente.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_incidente.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_incidente.cod_incidente).ToList();
            VarGlobal.pro_incidente.lst_lc_pro_evidencia = lista;
        }

        private void CargarPersonal()
        {
            lc_pro_incidente_personal_Data o_Data = new lc_pro_incidente_personal_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_incidente.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_incidente.cod_unidad
                                                && x.cod_incidente == VarGlobal.pro_incidente.cod_incidente).ToList();
            VarGlobal.pro_incidente.lst_lc_pro_incidente_personal = lista;
        }

        private void CargarSevReal(string s_cod)
        {
            lc_acc_unidad_severidadreal_Data o_Data = new lc_acc_unidad_severidadreal_Data();
            lst_real = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                    && x.cod_unidad == VarGlobal.cod_unidad
                                                    && x.cod_sisgestion == s_cod).ToList();
        }

        private void CargarSevPote(string s_cod)
        {
            lc_acc_unidad_severidadpot_Data o_Data = new lc_acc_unidad_severidadpot_Data();
            lst_potencial = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                    && x.cod_unidad == VarGlobal.cod_unidad
                                                    && x.cod_sisgestion == s_cod).ToList();
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            VarGlobal.pro_incidente.des_incidente = texto;
        }

        private void MostrarUbicacion()
        {
            ubicacion = VarGlobal.pro_incidente.cod_tipoubicacion;
            switch (VarGlobal.pro_incidente.cod_tipoubicacion)
            {
                case "E":
                    lbl_ubicacion.Text = VarGlobal.pro_incidente.nom_equipo;
                    VarGlobal.pro_incidente.cod_labor = "";
                    VarGlobal.pro_incidente.nom_labor = "";
                    VarGlobal.pro_incidente.cod_lugar = "";
                    VarGlobal.pro_incidente.nom_lugar = "";
                    break;
                case "I":
                    lbl_ubicacion.Text = VarGlobal.pro_incidente.nom_labor;
                    VarGlobal.pro_incidente.cod_lugar = "";
                    VarGlobal.pro_incidente.nom_lugar = "";
                    VarGlobal.pro_incidente.cod_equipo = "";
                    VarGlobal.pro_incidente.nom_equipo = "";
                    break;
                case "S":
                    lbl_ubicacion.Text = VarGlobal.pro_incidente.nom_lugar;
                    VarGlobal.pro_incidente.cod_labor = "";
                    VarGlobal.pro_incidente.nom_labor = "";
                    VarGlobal.pro_incidente.cod_equipo = "";
                    VarGlobal.pro_incidente.nom_equipo = "";
                    break;
            }
        }

        #region Fecha y Hora

        private void dpFecha_DateSelected(object sender, DateChangedEventArgs e)
        {
            var fecha = ((DatePicker)sender).Date;
            dpFechaLabel.Text = fecha.ToString("dd/MM/yyyy");
        }

        private void dpFechaLabel_Tapped(object sender, EventArgs e)
        {
            dpFecha.Focus();
        }

        private void pkHora_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ((Picker)sender).SelectedItem;
            pkHoraLabel.Text = item.ToString();

            VarGlobal.pro_incidente.hora = pkHoraLabel.Text + ":" + pkMinutoLabel.Text;
        }

        private void pkMinuto_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ((Picker)sender).SelectedItem;
            pkMinutoLabel.Text = item.ToString();

            VarGlobal.pro_incidente.hora = pkHoraLabel.Text + ":" + pkMinutoLabel.Text;
        }

        private void pkHoraLabel_Tapped(object sender, EventArgs e)
        {
            pkHora.Focus();
        }

        private void pkMinutoLabel_Tapped(object sender, EventArgs e)
        {
            pkMinuto.Focus();
        }
        #endregion

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
                VarGlobal.pro_incidente.cod_sisgestion = result;
                VarGlobal.pro_incidente.nom_sisgestion = lst_sisgestion.Where(x => x.cod_sisgestion == result).FirstOrDefault().nom_sisgestion.ToString();

                pkSistemaLabel.Text = VarGlobal.pro_incidente.nom_sisgestion;

                CargarSevReal(VarGlobal.pro_incidente.cod_sisgestion);
                CargarSevPote(VarGlobal.pro_incidente.cod_sisgestion);
                pkRealLabel.IsEnabled = true;
                btnReal.IsEnabled = true;
                pkPoteLabel.IsEnabled = true;
                btnPote.IsEnabled = true;
            }
        }

        private async void pkRealLabel_Tapped(object sender, EventArgs e)
        {
            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "REAL";
            for (int i = 0; i < lst_real.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_real[i].cod_severidadreal,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_real[i].nom_severidadreal
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_incidente.cod_severidadreal = result;
                VarGlobal.pro_incidente.nom_severidadreal = lst_real.Where(x => x.cod_severidadreal == result).FirstOrDefault().nom_severidadreal.ToString();

                pkRealLabel.Text = VarGlobal.pro_incidente.nom_severidadreal;
            }
        }

        private async void pkPoteLabel_Tapped(object sender, EventArgs e)
        {
            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "REAL";
            for (int i = 0; i < lst_potencial.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_potencial[i].cod_severidadpot,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_potencial[i].nom_severidadpot
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_incidente.cod_severidadpot = result;
                VarGlobal.pro_incidente.nom_severidadpot = lst_potencial.Where(x => x.cod_severidadpot == result).FirstOrDefault().nom_severidadpot.ToString();

                pkPoteLabel.Text = VarGlobal.pro_incidente.nom_severidadpot;
            }
        }

        private async void pkUbicacionLabel_Tapped(object sender, EventArgs e)
        {
            lc_acc_unidad_tipoubicacion_Data o_Data = new lc_acc_unidad_tipoubicacion_Data();
            var lst_ubicacion = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_unidad == VarGlobal.cod_unidad).ToList();
            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "UBICACIÓN";
            for (int i = 0; i < lst_ubicacion.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_ubicacion[i].cod_tipoubicacion,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_ubicacion[i].nom_tipoubicacion
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_incidente.cod_tipoubicacion = result;
                VarGlobal.pro_incidente.nom_tipoubicacion = lst_ubicacion.Where(x => x.cod_tipoubicacion == result).FirstOrDefault().nom_tipoubicacion.ToString();

                VarGlobal.cod_modulo_ret = "IN";
                switch (result)
                {
                    case "E":
                        ubicacion = "E";
                        await Navigation.PushAsync(new pg_equipo() { Title = "Seleccionar" });
                        break;
                    case "I":
                        ubicacion = "I";
                        await Navigation.PushAsync(new pg_labor() { Title = "Seleccionar" });
                        break;
                    case "S":
                        ubicacion = "S";
                        await Navigation.PushAsync(new pg_lugar() { Title = "Seleccionar" });
                        break;
                }
                pkUbicacionLabel.Text = VarGlobal.pro_incidente.nom_tipoubicacion;
            }
        }

        #endregion

        private void btn_reportado_Clicked(object sender, EventArgs e)
        {
            VarGlobal.retorno = "";
            VarGlobal.retorno = "pg_pro_incidente_mnt";
            var btn = ((Button)sender).CommandParameter;
            switch (btn)
            {
                case "Rep":
                    Navigation.PushAsync(new pg_personal("IN", "Rep") { Title = "Seleccionar" });
                    break;
                case "Part":
                    Navigation.PushAsync(new pg_personal("IN", "Part") { Title = "Seleccionar" });
                    break;
            }
        }

        private void btn_ubicacion_Clicked(object sender, EventArgs e)
        {
            VarGlobal.cod_modulo_ret = "IN";
            switch (ubicacion)
            {
                case "E":
                    Navigation.PushAsync(new pg_equipo());
                    break;
                case "I":
                    Navigation.PushAsync(new pg_labor());
                    break;
                case "S":
                    Navigation.PushAsync(new pg_lugar());
                    break;
            }
        }

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "WAR",
                tit_mensaje = "Incidente",
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
            string s_retorno = VarGlobal.pro_incidente.retorno;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));
        }

        private void IconButton_Clicked(object sender, EventArgs e)
        {
            VarGlobal.cod_modulo_ret = "IN";
            Navigation.PushAsync(new pg_evidencia() { Title = "Agregar Evidencia" });
        }
    }
}
