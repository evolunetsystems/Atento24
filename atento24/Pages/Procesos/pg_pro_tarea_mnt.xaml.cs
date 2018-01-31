using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Pages.Busquedas;
using atento24.Pages.Popup;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_tarea_mnt : ContentPage
    {
        //public pg_Loading loadingPage = new pg_Loading();
        public static string ubicacion;
        public pg_pro_tarea_mnt(string comando)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            LoadComando(comando);
            CargarDatos();
        }

        private void LoadComando(string comando)
        {
            lc_aux_estado_Data o_EstadoData = new lc_aux_estado_Data();

            DateTime fecha = DateTime.Now;
            switch (comando)
            {
                case "N":
                    //VarGlobal.pro_tarea.Limpiar();
                    //VarGlobal.pro_tarea = new lc_pro_tarea();
                    VarGlobal.pro_tarea.cod_empresa = VarGlobal.cod_empresa;
                    VarGlobal.pro_tarea.cod_unidad = VarGlobal.cod_unidad;
                    VarGlobal.pro_tarea.ini_tarea = fecha.ToString("dd/MM/yyyy");
                    VarGlobal.pro_tarea.fin_tarea = fecha.AddDays(4).ToString("dd/MM/yyyy");
                    VarGlobal.pro_tarea.sol_personal = VarGlobal.cod_personal;

                    //  Asignar Estado Inicial
                    VarGlobal.pro_tarea.cod_estado = "01";
                    VarGlobal.pro_tarea.nom_estado = o_EstadoData.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                            && x.cod_modulo == "TR"
                                                                            && x.cod_estado == "01").FirstOrDefault().nom_estado;

                    lbl_solicitante.Text = VarGlobal.nom_personal;
                    lbl_ubicacion.Text = VarGlobal.pro_tarea.nom_ubicacion;
                    MostrarUbicacion();
                    break;
                case "B":
                    lbl_reportado.Text = VarGlobal.pro_tarea.nom_eje_personal;
                    edDescrip.Text = VarGlobal.pro_tarea.des_tarea;
                    lbl_solicitante.Text = VarGlobal.nom_personal;
                    MostrarUbicacion();
                    break;
                case "M":
                    lbl_reportado.Text = VarGlobal.pro_tarea.nom_eje_personal;
                    edDescrip.Text = VarGlobal.pro_tarea.des_tarea;
                    lbl_solicitante.Text = VarGlobal.pro_tarea.nom_sol_personal;
                    lbl_ubicacion.Text = VarGlobal.pro_tarea.nom_ubicacion;
                    break;
            }
        }

        private void CargarDatos()
        {
            dpFechaIniLabel.Text = VarGlobal.pro_tarea.ini_tarea;
            dpFechaFinLabel.Text = VarGlobal.pro_tarea.fin_tarea;
            pkUbicacionLabel.Text = VarGlobal.pro_tarea.nom_tipoubicacion;
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
                VarGlobal.pro_tarea.cod_tipoubicacion = result;
                VarGlobal.pro_tarea.nom_tipoubicacion = lst_ubicacion.Where(x => x.cod_tipoubicacion == result).FirstOrDefault().nom_tipoubicacion.ToString();

                VarGlobal.cod_modulo_ret = "TR";
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
                pkUbicacionLabel.Text = VarGlobal.pro_tarea.nom_tipoubicacion;
            }
        }

        private void btn_ubicacion_Clicked(object sender, EventArgs e)
        {
            VarGlobal.cod_modulo_ret = "TR";
            switch (VarGlobal.pro_tarea.cod_tipoubicacion)
            {
                case "E":
                    Navigation.PushAsync(new pg_equipo() { Title = "Seleccionar" });
                    break;
                case "I":
                    Navigation.PushAsync(new pg_labor() { Title = "Seleccionar" });
                    break;
                case "S":
                    Navigation.PushAsync(new pg_lugar() { Title = "Seleccionar" });
                    break;
            }
        }

        private void MostrarUbicacion()
        {
            switch (VarGlobal.pro_tarea.cod_tipoubicacion)
            {
                case "E":
                    lbl_ubicacion.Text = VarGlobal.pro_tarea.nom_equipo;
                    VarGlobal.pro_tarea.cod_labor = "";
                    VarGlobal.pro_tarea.nom_labor = "";
                    VarGlobal.pro_tarea.cod_lugar = "";
                    VarGlobal.pro_tarea.nom_lugar = "";
                    break;
                case "I":
                    lbl_ubicacion.Text = VarGlobal.pro_tarea.nom_labor;
                    VarGlobal.pro_tarea.cod_lugar = "";
                    VarGlobal.pro_tarea.nom_lugar = "";
                    VarGlobal.pro_tarea.cod_equipo = "";
                    VarGlobal.pro_tarea.nom_equipo = "";
                    break;
                case "S":
                    lbl_ubicacion.Text = VarGlobal.pro_tarea.nom_lugar;
                    VarGlobal.pro_tarea.cod_labor = "";
                    VarGlobal.pro_tarea.nom_labor = "";
                    VarGlobal.pro_tarea.cod_equipo = "";
                    VarGlobal.pro_tarea.nom_equipo = "";
                    break;
            }
        }

        #region FECHAS
        private void dpFechaIni_DateSelected(object sender, DateChangedEventArgs e)
        {
            var fecha = ((DatePicker)sender).Date;
            dpFechaIniLabel.Text = fecha.ToString("dd/MM/yyyy");
            VarGlobal.pro_tarea.ini_tarea = fecha.ToString("dd/MM/yyyy");
        }

        private void dpFechaIniLabel_Tapped(object sender, EventArgs e)
        {
            dpFechaIni.Focus();
        }

        private void dpFechaFin_DateSelected(object sender, DateChangedEventArgs e)
        {
            var fecha = ((DatePicker)sender).Date;
            dpFechaFinLabel.Text = fecha.ToString("dd/MM/yyyy");
            VarGlobal.pro_tarea.fin_tarea = fecha.ToString("dd/MM/yyyy");
        }

        private void dpFechaFinLabel_Tapped(object sender, EventArgs e)
        {
            dpFechaFin.Focus();
        }
        #endregion

        private async void btn_grabar_Clicked(object sender, EventArgs e)
        {
            var loadingPage = new pg_Loading();
            if (!ValidarAccion())
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "INF",
                    tit_mensaje = "Tárea",
                    tex_mensaje = "¿Desea Grabar Registro?"
                });
                var result = await popupAlert.Show();
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_tarea_Data o_Data = new lc_pro_tarea_Data();
                    //  Generar Codigo, solo si viene VACIO O NULO
                    if (string.IsNullOrEmpty(VarGlobal.pro_tarea.cod_tarea))
                    {
                        var conteo = (o_Data.Listar().Count) + 1;
                        var año = VarGlobal.pro_tarea.ini_tarea.Substring(8, 2);
                        var mes = VarGlobal.pro_tarea.ini_tarea.Substring(3, 2);
                        VarGlobal.pro_tarea.cod_tarea = "TR" + año + mes + "-" + conteo;
                        VarGlobal.pro_tarea.nom_eje_personal = lbl_reportado.Text;
                        VarGlobal.pro_tarea.nom_sol_personal = lbl_solicitante.Text;
                    }
                    VarGlobal.pro_tarea.sincronizado = false;
                    o_Data.Modificar(VarGlobal.pro_tarea);
                    Retornar();
                }
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            var loadingPage = new pg_Loading();
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "WAR",
                tit_mensaje = "Tárea",
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

        private void Retornar()
        {
            string s_retorno = VarGlobal.ret_tarea_hijo;
            Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));
        }

        private void btn_reportado_Clicked(object sender, EventArgs e)
        {
            var btn = ((Button)sender).CommandParameter;
            var modulo = VarGlobal.pro_tarea.cod_modulo;
            switch (btn)
            {
                case "Eje":
                    Navigation.PushAsync(new pg_personal(modulo, "Eje") { Title = "Seleccionar" });
                    break;
            }
        }

        private void edDescrip_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            VarGlobal.pro_tarea.des_tarea = texto;
        }

        private bool ValidarAccion()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(lbl_reportado.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Ejecutor...";
            }

            if (string.IsNullOrEmpty(edDescrip.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Que Hacer...";
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
    }
}
