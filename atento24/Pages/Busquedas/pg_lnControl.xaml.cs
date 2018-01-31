using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using atento24.Pages.Procesos;

namespace atento24.Pages.Busquedas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_lnControl : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public static string s_comando;
        public static int i_orden;
        public static int i_tot_orden;
        public pg_lnControl(string comando, int orden)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            s_comando = comando;
            i_orden = orden;
            CargarCabecera();
            CalcularCumplimiento();

            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Where(x => x.ord_lncontrol == orden).ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].cod_tipodato == "02")
                {
                    btnCumple.IsEnabled = false;
                    btnNoCumple.IsEnabled = false;
                }
            }

            if (i_orden > i_tot_orden)
            {
                btnNext.IsVisible = false;
                btnGrabar.IsVisible = true;
                slCumplimiento.IsVisible = false;
                slLnControl.IsVisible = false;
                slGrabar.IsVisible = true;
                lbl_lncontrol.Text = "Fin";
                slCoor.IsVisible = true;
                //stCoo.IsVisible = true;
                //stMapa.IsVisible = true;
                lbl_cum_lncontrol.Text = Convert.ToString(VarGlobal.pro_veoregistro.cum_veoregistro);
                lbl_noc_lncontrol.Text = Convert.ToString(VarGlobal.pro_veoregistro.noc_veoregistro);
                lbl_noa_lncontrol.Text = Convert.ToString(VarGlobal.pro_veoregistro.noa_veoregistro);
                int i_por_veoregistro = Convert.ToInt32(VarGlobal.pro_veoregistro.por_veoregistro * 100);
                lbl_por_lncontrol.Text = Convert.ToString(i_por_veoregistro) + " %";
                AsignarCoordenada();
            }
        }

        private async void AsignarCoordenada()
        {
            var position = await CrossGeolocator.Current.GetPositionAsync();
            VarGlobal.pro_veoregistro.latitud = Convert.ToDecimal(position.Latitude);
            VarGlobal.pro_veoregistro.longitud = Convert.ToDecimal(position.Longitude);

            var position1 = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            var pin = new Pin { Type = PinType.Place, Position = position1, Label = "Soy un Pin" };

            //  Asignar Posicion.
            MapSpan mp = MapSpan.FromCenterAndRadius(position1, Distance.FromKilometers(1));

            Mapa.MoveToRegion(mp);
            MostrarPins();
        }

        private void MostrarPins()
        {
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_coordenada;
            for (int i = 0; i < lista.Count; i++)
            {
                var position = new Xamarin.Forms.Maps.Position(Convert.ToDouble(lista[i].lat_coordenada), Convert.ToDouble(lista[i].lon_coordenada));
                var pin = new Pin { Type = PinType.Place, Position = position, Label = lista[i].com_coordenada };
                Mapa.Pins.Add(pin);
            }
        }

        private async void CargarCabecera()
        {
            i_tot_orden = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Count;
            lbl_fecha.Text = VarGlobal.pro_veoregistro.fec_veoregistro;
            lbl_nompla.Text = VarGlobal.pro_veoregistro.nom_veoplantilla;
            lbl_Lugar.Text = VarGlobal.pro_veoregistro.nom_tipoubicacion;
            switch (VarGlobal.pro_veoregistro.cod_tipoubicacion)
            {
                case "E":
                    lbl_Ubicacion.Text = VarGlobal.pro_veoregistro.nom_equipo;
                    break;
                case "I":
                    lbl_Ubicacion.Text = VarGlobal.pro_veoregistro.nom_labor;
                    break;
                case "S":
                    lbl_Ubicacion.Text = VarGlobal.pro_veoregistro.nom_lugar;
                    break;
            }

            var filtro = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Where(x => x.ord_lncontrol == i_orden).ToList();
            for (int i = 0; i < filtro.Count; i++)
            {
                lbl_lncontrol.Text = Convert.ToString(filtro[i].ord_lncontrol) + "/" + Convert.ToString(i_tot_orden);
                lbl_nom_lncontrol.Text = filtro[i].nom_lncontrol;
                lbl_val_lncontrol.Text = Convert.ToString(filtro[i].val_lncontrol);
                lbl_nom_simbolo.Text = filtro[i].nom_simbolo;
                edComentario.Text = filtro[i].com_lncontrol;

                //
                if (filtro[i].val_lncontrol != 0)
                {
                    lbl_val_lncontrol.IsVisible = true;
                    lbl_nom_simbolo.IsVisible = true;

                    var valor = filtro[i].val_lncontrol;
                    var pa1 = filtro[i].pa1_lncontrol;
                    var pa2 = filtro[i].pa2_lncontrol;

                    switch (filtro[i].cod_simbolo)
                    {
                        case "01":
                            if (valor == pa1)
                            {
                                filtro[i].cum_lncontrol = 1;
                                filtro[i].noc_lncontrol = 0;
                                filtro[i].noa_lncontrol = 0;
                                filtro[i].com_lncontrol = "";
                                lblComentario.IsVisible = false;
                                edComentario.IsVisible = false;
                                //botones
                                btnCumple.Text = "fa-check-square-o";
                                btnNoCumple.Text = "fa-square-o";
                                btnNoAplica.Text = "fa-square-o";

                            }
                            else
                            {
                                filtro[i].cum_lncontrol = 0;
                                filtro[i].noc_lncontrol = 1;
                                filtro[i].noa_lncontrol = 0;
                                lblComentario.IsVisible = true;
                                edComentario.IsVisible = true;
                                //botones
                                btnCumple.Text = "fa-square-o";
                                btnNoCumple.Text = "fa-check-square-o";
                                btnNoAplica.Text = "fa-square-o";
                                if (filtro[i].ale_lncontrol.Trim().Length > 0)
                                {
                                    await DisplayAlert("NO CUMPLE", filtro[i].ale_lncontrol, "", "Ok");
                                }
                            }
                            break;
                        case "02":
                            if (valor > pa1)
                            {
                                filtro[i].cum_lncontrol = 1;
                                filtro[i].noc_lncontrol = 0;
                                filtro[i].noa_lncontrol = 0;
                                filtro[i].com_lncontrol = "";
                                lblComentario.IsVisible = false;
                                edComentario.IsVisible = false;
                                //botones
                                btnCumple.Text = "fa-check-square-o";
                                btnNoCumple.Text = "fa-square-o";
                                btnNoAplica.Text = "fa-square-o";
                            }
                            else
                            {
                                filtro[i].cum_lncontrol = 0;
                                filtro[i].noc_lncontrol = 1;
                                filtro[i].noa_lncontrol = 0;
                                lblComentario.IsVisible = true;
                                edComentario.IsVisible = true;
                                //botones
                                btnCumple.Text = "fa-square-o";
                                btnNoCumple.Text = "fa-check-square-o";
                                btnNoAplica.Text = "fa-square-o";
                                if (filtro[i].ale_lncontrol.Trim().Length > 0)
                                {
                                    await DisplayAlert("NO CUMPLE", filtro[i].ale_lncontrol, "", "Ok");
                                }
                            }
                            break;
                        case "03":
                            if (valor < pa1)
                            {
                                filtro[i].cum_lncontrol = 1;
                                filtro[i].noc_lncontrol = 0;
                                filtro[i].noa_lncontrol = 0;
                                filtro[i].com_lncontrol = "";
                                lblComentario.IsVisible = false;
                                edComentario.IsVisible = false;
                                //botones
                                btnCumple.Text = "fa-check-square-o";
                                btnNoCumple.Text = "fa-square-o";
                                btnNoAplica.Text = "fa-square-o";
                            }
                            else
                            {
                                filtro[i].cum_lncontrol = 0;
                                filtro[i].noc_lncontrol = 1;
                                filtro[i].noa_lncontrol = 0;
                                lblComentario.IsVisible = true;
                                edComentario.IsVisible = true;
                                //botones
                                btnCumple.Text = "fa-square-o";
                                btnNoCumple.Text = "fa-check-square-o";
                                btnNoAplica.Text = "fa-square-o";
                                if (filtro[i].ale_lncontrol.Trim().Length > 0)
                                {
                                    await DisplayAlert("NO CUMPLE", filtro[i].ale_lncontrol, "", "Ok");
                                }
                            }
                            break;
                        case "04":
                            if (valor < pa1 && pa1 < pa2)
                            {
                                filtro[i].cum_lncontrol = 1;
                                filtro[i].noc_lncontrol = 0;
                                filtro[i].noa_lncontrol = 0;
                                filtro[i].com_lncontrol = "";
                                lblComentario.IsVisible = false;
                                edComentario.IsVisible = false;
                                //botones
                                btnCumple.Text = "fa-check-square-o";
                                btnNoCumple.Text = "fa-square-o";
                                btnNoAplica.Text = "fa-square-o";
                            }
                            else
                            {
                                filtro[i].cum_lncontrol = 0;
                                filtro[i].noc_lncontrol = 1;
                                filtro[i].noa_lncontrol = 0;
                                lblComentario.IsVisible = true;
                                edComentario.IsVisible = true;
                                //botones
                                btnCumple.Text = "fa-square-o";
                                btnNoCumple.Text = "fa-check-square-o";
                                btnNoAplica.Text = "fa-square-o";
                                if (filtro[i].ale_lncontrol.Trim().Length > 0)
                                {
                                    await DisplayAlert("NO CUMPLE", filtro[i].ale_lncontrol, "", "Ok");
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (filtro[i].cum_lncontrol == 1)
                    {
                        btnCumple.Text = "fa-check-square-o";
                    }
                    else
                    {
                        btnCumple.Text = "fa-square-o";
                    }

                    if (filtro[i].noc_lncontrol == 1)
                    {
                        btnNoCumple.Text = "fa-check-square-o";
                        lblComentario.IsVisible = true;
                        edComentario.IsVisible = true;
                        if (string.IsNullOrEmpty(filtro[i].ale_lncontrol)) { }
                        else
                        {
                            VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                            {
                                tip_mensaje = "INF",
                                tit_mensaje = "Informativo",
                                tex_mensaje = filtro[i].ale_lncontrol
                            });
                            await Navigation.PushPopupAsync(VarGlobal._mensaje);
                        }
                    }
                    else
                    {
                        btnNoCumple.Text = "fa-square-o";
                        lblComentario.IsVisible = false;
                        edComentario.IsVisible = false;
                    }

                    if (filtro[i].noa_lncontrol == 1)
                    {
                        btnNoAplica.Text = "fa-check-square-o";
                        lbl_val_lncontrol.IsVisible = true;
                        lbl_nom_simbolo.IsVisible = true;
                    }
                    else
                    {
                        btnNoAplica.Text = "fa-square-o";
                        lbl_val_lncontrol.IsVisible = false;
                        lbl_nom_simbolo.IsVisible = false;
                    }
                }
                edComentario.Text = filtro[i].com_lncontrol;
            }
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Where(x => x.ord_lncontrol == i_orden).FirstOrDefault();

            if (lista.noc_lncontrol == 1)
            {
                if (!ValidarLineaControl())
                {
                    if (i_orden > i_tot_orden)
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("M") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    else
                    {
                        int orden = i_orden + 1;
                        Navigation.PushAsync(new pg_lnControl(s_comando, orden) { Title = "Lineas de Control" });
                    }
                }
            }
            else
            {
                if (i_orden > i_tot_orden)
                {
                    Navigation.PushAsync(new pg_pro_veoregistro_mnt("M") { Title = VarGlobal.pro_veoregistro.titulo });
                }
                else
                {
                    int orden = i_orden + 1;
                    Navigation.PushAsync(new pg_lnControl(s_comando, orden) { Title = "Lineas de Control" });
                }
            }
        }

        private async void btnSalir_Clicked(object sender, EventArgs e)
        {
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
            int orden = i_orden - 1;
            if (orden == 0)
            {
                if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
                {
                    await Navigation.PushAsync(new pg_pro_veoregistro_mnt("N") { Title = VarGlobal.pro_veoregistro.titulo });
                }
                else
                {
                    await Navigation.PushAsync(new pg_pro_veoregistro_mnt("M") { Title = VarGlobal.pro_veoregistro.titulo });
                }

            }
            else
            {
                await Navigation.PushAsync(new pg_lnControl(s_comando, orden) { Title = "Lineas de Control" });
            }

        }

        private async void btn_grabar_Clicked(object sender, EventArgs e)
        {
            //var popupAlert = new pg_confirmacion("V.E.O.", "¿Desea Grabar?");
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "INF",
                tit_mensaje = "V.E.O.",
                tex_mensaje = "¿Desea Grabar Registro?"
            });
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                lc_pro_veoregistro_Data o_Data = new lc_pro_veoregistro_Data();
                //  Generar Codigo, solo si viene VACIO O NULO
                if (string.IsNullOrEmpty(VarGlobal.pro_veoregistro.cod_veoregistro))
                {
                    var conteo = (o_Data.Listar().Count) + 1;
                    var año = VarGlobal.pro_veoregistro.fec_veoregistro.Substring(8, 2);
                    var mes = VarGlobal.pro_veoregistro.fec_veoregistro.Substring(3, 2);
                    string s_codigo = VarGlobal.cod_modulo + año + mes + "-" + conteo;

                    VarGlobal.pro_veoregistro.cod_veoregistro = s_codigo;

                }
                //VarGlobal.pro_veoregistro.por_veoregistro = (VarGlobal.pro_veoregistro.por_veoregistro) * 100;
                VarGlobal.pro_veoregistro.sincronizado = false;
                o_Data.EliminarUno(VarGlobal.pro_veoregistro);
                o_Data.Insertar(VarGlobal.pro_veoregistro);

                string s_retorno = VarGlobal.pro_veoregistro.retorno;
                await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));
            }
            await Navigation.RemovePopupPageAsync(loadingPage);
        }

        private void btnCumple_Clicked(object sender, EventArgs e)
        {
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
            for (int i = 0; i < lista.Count; i++)
            {
                if (i_orden == lista[i].ord_lncontrol)
                {
                    lista[i].cum_lncontrol = 1;
                    lista[i].noc_lncontrol = 0;
                    lista[i].noa_lncontrol = 0;
                    lista[i].com_lncontrol = "";
                }
            }
            CargarCabecera();
            CalcularCumplimiento();
        }

        private void btnNoCumple_Clicked(object sender, EventArgs e)
        {
            var mensaje = "";
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
            for (int i = 0; i < lista.Count; i++)
            {
                if (i_orden == lista[i].ord_lncontrol)
                {
                    lista[i].cum_lncontrol = 0;
                    lista[i].noc_lncontrol = 1;
                    lista[i].noa_lncontrol = 0;
                    mensaje = lista[i].ale_lncontrol;
                }
            }
            CargarCabecera();
            CalcularCumplimiento();

            //await DisplayAlert("NO CUMPLE", mensaje, "", "Ok");
        }

        private void btnNoAplica_Clicked(object sender, EventArgs e)
        {
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
            for (int i = 0; i < lista.Count; i++)
            {
                if (i_orden == lista[i].ord_lncontrol)
                {
                    lista[i].cum_lncontrol = 0;
                    lista[i].noc_lncontrol = 0;
                    lista[i].noa_lncontrol = 1;
                    lista[i].com_lncontrol = "";
                    lista[i].val_lncontrol = 0;
                }
            }
            CargarCabecera();
            CalcularCumplimiento();

            //pasamos a la siguiente linea de control
            int orden = i_orden + 1;
            Navigation.PushAsync(new pg_lnControl(s_comando, orden) { Title = "Lineas de Control" });
        }

        private void edComentario_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;
            for (int i = 0; i < lista.Count; i++)
            {
                if (i_orden == lista[i].ord_lncontrol)
                {
                    lista[i].com_lncontrol = texto;
                }
            }
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            var texto = ((Entry)sender).Text;
            var lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Where(x => x.ord_lncontrol == i_orden).ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].val_lncontrol = Convert.ToDecimal(texto);
            }
            CargarCabecera();
            CalcularCumplimiento();
        }

        private void CalcularCumplimiento()
        {
            List<lc_pro_veoregistro_lncontrol> lista = VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol;

            int c = 0, nc = 0, na = 0; int sum = 0;
            decimal d_cons = Convert.ToDecimal(1.00);

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].cum_lncontrol == 1)
                {
                    c = c + 1;
                }
                if (lista[i].noc_lncontrol == 1)
                {
                    nc = nc + 1;
                }
                if (lista[i].noa_lncontrol == 1)
                {
                    na = na + 1;
                }
            }
            VarGlobal.pro_veoregistro.cum_veoregistro = c;
            VarGlobal.pro_veoregistro.noc_veoregistro = nc;
            VarGlobal.pro_veoregistro.noa_veoregistro = na;
            if ((c + nc).ToString() == "0")
            { sum = 1; }
            else
            { sum = c + nc; }

            VarGlobal.pro_veoregistro.por_veoregistro = (c * d_cons / (sum));
        }

        private bool ValidarLineaControl()
        {
            bool b_Error = false;
            string s_Mensaje = ""; //

            if (string.IsNullOrEmpty(edComentario.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Ingresar Comentario...";
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
