using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Busquedas;
using atento24.Pages.Popup;
using atento24.Recursos;
using Plugin.Geolocator;
using Plugin.Iconize;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_veoregistro_mnt : ContentPage
    {
        public static string s_comando;
        public pg_Loading loadingPage = new pg_Loading();
        //public static List<pro_participante> lst_evaluadores;
        public pg_pro_veoregistro_mnt(string comando)
        {
            InitializeComponent();
            OnBackButtonPressed();
            NavigationPage.SetHasBackButton(this, false);
            s_comando = comando;
            DateTime fecha = dpFecha.Date;
            var hora = DateTime.Now.TimeOfDay;
            if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
            {
                btn_grabar.IsVisible = false;
            }
            else
            {
                btn_grabar.IsVisible = true;
            }
            switch (comando)
            {
                case "N":
                    VarGlobal.pro_veoregistro.lst_lc_pro_coordenada = new List<lc_pro_coordenada>();
                    VarGlobal.pro_veoregistro.fec_veoregistro = (fecha.ToString("dd/MM/yyyy") + " " + hora).Substring(0, 19);
                    VarGlobal.pro_veoregistro.hora = hora.ToString();
                    VarGlobal.pro_veoregistro.cod_empresa = VarGlobal.cod_empresa;
                    VarGlobal.pro_veoregistro.cod_unidad = VarGlobal.cod_unidad;
                    if (s_comando == "N")
                    {
                        var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
                        for (int i = 0; i < VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Count; i++)
                        {
                            if (lista[i].cod_tipodato == "01")
                            {
                                lista[i].cum_lncontrol = 1;
                            }
                            else
                            {
                                lista[i].noa_lncontrol = 1;
                            }
                        }
                    }
                    AsignarCoordenada();
                    break;
                case "B":
                    if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
                    {
                        s_comando = "N";
                    }
                    else
                    {
                        s_comando = "M";
                    }
                    break;
                case "M":
                    CargarEvaluadores();
                    CargarLineasControl();
                    CargarCoordenadas();
                    AsignarCoordenada();
                    break;
            }
            CargarCabecera();
            MostrarEvaluadores();
            MostrarUbicacion();
        }

        private void CargarCoordenadas()
        {
            lc_pro_coordenada_Data o_Data = new lc_pro_coordenada_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                && x.cod_unidad == VarGlobal.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_veoregistro.cod_veoregistro
                                                && x.cod_modulo == VarGlobal.cod_modulo).ToList();
            VarGlobal.pro_veoregistro.lst_lc_pro_coordenada = lista;
        }

        private async void AsignarCoordenada()
        {
            var position = await CrossGeolocator.Current.GetPositionAsync();
            VarGlobal.pro_veoregistro.latitud = Convert.ToDecimal(position.Latitude);
            VarGlobal.pro_veoregistro.longitud = Convert.ToDecimal(position.Longitude);

            var position1 = new Position(position.Latitude, position.Longitude);
            var pin = new Pin { Type = PinType.Place, Position = position1, Label = "Soy un Pin" };

            InsertarCoordenada();
        }

        private void InsertarCoordenada()
        {
            var hora = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
            var fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
            string s_creado = "";
            if (s_comando == "N")
            {
                s_creado = "Nuevo " + fecha + "-" + hora + " Por " + VarGlobal.nom_personal;
            }
            else
            {
                s_creado = "Modificado " + fecha + "-" + hora + " Por " + VarGlobal.nom_personal;
            }
            VarGlobal.pro_veoregistro.lst_lc_pro_coordenada.Add(new lc_pro_coordenada()
            {
                cod_empresa = VarGlobal.cod_empresa,
                cod_unidad = VarGlobal.cod_unidad,
                cod_referencia = VarGlobal.pro_veoregistro.cod_veoregistro,
                cod_modulo = VarGlobal.cod_modulo,
                num_coordenada = 0,
                lat_coordenada = VarGlobal.pro_veoregistro.latitud,
                lon_coordenada = VarGlobal.pro_veoregistro.longitud,
                com_coordenada = s_creado,
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip,
                estado = "A",
            });
        }

        private void CargarLineasControl()
        {
            lc_pro_veoregistro_lncontrol_Data o_Data = new lc_pro_veoregistro_lncontrol_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_veoregistro.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_veoregistro.cod_unidad
                                                && x.cod_veoregistro == VarGlobal.pro_veoregistro.cod_veoregistro
                                                ).ToList();
            VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol = lista;


        }

        private void CargarEvaluadores()
        {
            lc_pro_participante_Data o_Data = new lc_pro_participante_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_veoregistro.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_veoregistro.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_veoregistro.cod_veoregistro
                                                && x.tip_participante == "E").ToList();
            VarGlobal.pro_veoregistro.lst_lc_pro_participante = lista;
        }

        private void CargarCabecera()
        {
            dpFechaLabel.Text = VarGlobal.pro_veoregistro.fec_veoregistro.Substring(0, 10);
            pkHoraLabel.Text = VarGlobal.pro_veoregistro.fec_veoregistro.Substring(11, 2);
            pkMinutoLabel.Text = VarGlobal.pro_veoregistro.fec_veoregistro.Substring(14, 2);
            lbl_nomplantilla.Text = VarGlobal.pro_veoregistro.nom_veoplantilla;
            lbl_ubicacion.Text = VarGlobal.pro_veoregistro.nom_tipoubicacion;
        }

        private void MostrarLineaControl()
        {
            //fa-square-o
            //fa-check-square-o
            List<lc_pro_veoregistro_lncontrol> lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].noc_lncontrol == 1) { lista[i].vis_lncontrol = true; } else { lista[i].vis_lncontrol = false; }
                if (lista[i].noa_lncontrol == 1) { lista[i].ena_simbolo = true; } else { lista[i].ena_simbolo = false; }

                //deshabilitando valor simbolo
                if (s_comando == "N")
                {
                    if (lista[i].cod_tipodato == "01")
                    {
                        lista[i].ena_simbolo = false;
                        lista[i].cum_lncontrol = 1;
                    }
                    else
                    {
                        lista[i].ena_simbolo = true;
                        lista[i].noa_lncontrol = 1;
                    }
                }

                if (lista[i].cum_lncontrol == 1)
                {
                    lista[i].ib_check = "fa-check-square-o";
                }
                else
                {
                    lista[i].ib_check = "fa-square-o";
                }

                if (lista[i].noc_lncontrol == 1)
                {
                    lista[i].ib_check_nc = "fa-check-square-o";
                }
                else
                {
                    lista[i].ib_check_nc = "fa-square-o";
                }

                if (lista[i].noa_lncontrol == 1)
                {
                    lista[i].ib_check_na = "fa-check-square-o";
                }
                else
                {
                    lista[i].ib_check_na = "fa-square-o";
                }
            }
            //lstCheck.ItemsSource = lista;
        }

        private void MostrarEvaluadores()
        {
            List<lc_pro_participante> lista = VarGlobal.pro_veoregistro.lst_lc_pro_participante;
            if (lista != null)
            {
                //  Creando StackLayout Contenedor.
                for (int i = 0; i < lista.Count; i++)
                {
                    IconButton btnEli = new IconButton
                    {
                        WidthRequest = 40,
                        //Text = "fa-times",
                        Text = "fa-trash-o",
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

        private void btnEliInfractorClicked(object sender, EventArgs e)
        {
            var s_cod_personal = ((Button)sender).CommandParameter;
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_participante;
            for (int i = 0; i < lista.Count; i++)
            {
                string s_cod = lista[i].cod_personal;
                if (s_cod == s_cod_personal.ToString())
                {
                    lista.RemoveAt(i);
                    i--;
                }
            }
            VarGlobal.pro_veoregistro.lst_lc_pro_participante = lista;
            stInf.Children.Remove(stInf);
            Navigation.PushAsync(new pg_pro_veoregistro_mnt("B") { Title = VarGlobal.pro_veoregistro.titulo });
        }

        private void MostrarUbicacion()
        {
            switch (VarGlobal.pro_veoregistro.cod_tipoubicacion)
            {
                case "E":
                    UbicacionLabel.Text = VarGlobal.pro_veoregistro.nom_equipo;
                    VarGlobal.pro_veoregistro.cod_labor = "";
                    VarGlobal.pro_veoregistro.nom_labor = "";
                    VarGlobal.pro_veoregistro.cod_lugar = "";
                    VarGlobal.pro_veoregistro.nom_lugar = "";
                    break;
                case "I":
                    UbicacionLabel.Text = VarGlobal.pro_veoregistro.nom_labor;
                    VarGlobal.pro_veoregistro.cod_lugar = "";
                    VarGlobal.pro_veoregistro.nom_lugar = "";
                    VarGlobal.pro_veoregistro.cod_equipo = "";
                    VarGlobal.pro_veoregistro.nom_equipo = "";
                    break;
                case "S":
                    UbicacionLabel.Text = VarGlobal.pro_veoregistro.nom_lugar;
                    VarGlobal.pro_veoregistro.cod_labor = "";
                    VarGlobal.pro_veoregistro.nom_labor = "";
                    VarGlobal.pro_veoregistro.cod_equipo = "";
                    VarGlobal.pro_veoregistro.nom_equipo = "";
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

            VarGlobal.pro_veoregistro.hora = pkHoraLabel.Text + ":" + pkMinutoLabel.Text;
        }

        private void pkMinuto_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ((Picker)sender).SelectedItem;
            pkMinutoLabel.Text = item.ToString();

            VarGlobal.pro_veoregistro.hora = pkHoraLabel.Text + ":" + pkMinutoLabel.Text;
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

        private void btnEvaluador_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pg_personal("VE", "Eval") { Title = "Seleccionar" });
        }

        private void UbicacionLabel_Tapped(object sender, EventArgs e)
        {
            VarGlobal.cod_modulo_ret = "VE";
            switch (VarGlobal.pro_veoregistro.cod_tipoubicacion)
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

        private async void btn_grabar_Clicked(object sender, EventArgs e)
        {
            if (!ValidarVEO())
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "INF",
                    tit_mensaje = "V.E.O.",
                    tex_mensaje = "¿Desea Grabar Registro?"
                });
                //var popupAlert = new pg_confirmacion("V.E.O.", "¿Desea Grabar?");
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    lc_pro_veoregistro_Data o_Data = new lc_pro_veoregistro_Data();
                    VarGlobal.pro_veoregistro.sincronizado = false;
                    o_Data.EliminarUno(VarGlobal.pro_veoregistro);
                    o_Data.Insertar(VarGlobal.pro_veoregistro);
                    Retornar();
                }
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        private bool ValidarVEO()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(UbicacionLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Detalle ubicación...";
            }

            if (string.IsNullOrEmpty(lbl_nomplantilla.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Ingresar Plantilla...";
            }

            if (VarGlobal.pro_veoregistro.lst_lc_pro_participante.Count == 0 && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Ingresar Evaluador...";
            }

            if (VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Count == 0 && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Ingresar Lineas de Control...";
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

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "WAR",
                tit_mensaje = "V.E.O.",
                tex_mensaje = "¿Seguro que deseas salir?"
            });
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                VarGlobal.pro_veoregistro.lst_lc_pro_participante = new List<lc_pro_participante>();
                VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol = new List<lc_pro_veoregistro_lncontrol>();

                string s_retorno = VarGlobal.pro_veoregistro.retorno;
                await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));
            }
            await Navigation.RemovePopupPageAsync(loadingPage);
        }

        private async void Retornar()
        {
            //Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            //await Navigation.PushPopupAsync(loadingPage);

            string s_retorno = VarGlobal.pro_veoregistro.retorno;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));

            //await Navigation.RemovePopupPageAsync(loadingPage);
            //Content.IsEnabled = true;
        }

        private async void lncontrol_Clicked(object sender, EventArgs e)
        {
            if (!ValidarVEO())
            {
                Content.IsEnabled = false;
                var loadingPage = new pg_Loading();
                await Navigation.PushPopupAsync(loadingPage);

                await Navigation.PushAsync(new pg_lnControl(s_comando, 1) { Title = "Lineas de Control" });

                await Navigation.RemovePopupPageAsync(loadingPage);
                Content.IsEnabled = true;

            }
        }
    }
}
