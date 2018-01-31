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
    public partial class pg_pro_hallazgo_mnt : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();
        public static int count_coord;
        public static string s_comando;
        public static string ubicacion;
        //public static List<lc_pro_participante> lst_participante;
        //public static List<lc_pro_evidencia> lst_evidencia;

        public pg_pro_hallazgo_mnt(string comando)
        {
            InitializeComponent();
            OnBackButtonPressed();
            NavigationPage.SetHasBackButton(this, false);
            //s_comando = comando; 
            if (VarGlobal.pro_hallazgo.cod_modulo == "OB")
            {
                slHallazgo.IsVisible = false;
                pkHallazgoLabel.Text = "OBSERVACIÓN";
                VarGlobal.pro_hallazgo.cod_hallazgoclase = "01";
            }
            else
            {
                slHallazgo.IsVisible = true;
            }
            CargarVariables();

            LoadComando(comando);
            MostrarListaInfractores();
            MostrarEvidencias();
        }

        private void LoadComando(string comando)
        {
            DateTime fecha = dpFecha.Date;
            var hora = DateTime.Now.TimeOfDay.ToString();

            switch (comando)
            {
                case "N":

                    //VarGlobal.pro_hallazgo = new lc_pro_hallazgo();
                    VarGlobal.pro_hallazgo.cod_empresa = VarGlobal.cod_empresa;
                    VarGlobal.pro_hallazgo.cod_unidad = VarGlobal.cod_unidad;
                    VarGlobal.pro_hallazgo.fec_hallazgo = (fecha.ToString("dd/MM/yyyy") + " " + hora).Substring(0, 19);
                    VarGlobal.pro_hallazgo.cod_estado = "01";
                    VarGlobal.pro_hallazgo.lst_lc_pro_participante = new List<lc_pro_participante>();
                    VarGlobal.pro_hallazgo.lst_lc_pro_evidencia = new List<lc_pro_evidencia>();
                    VarGlobal.pro_hallazgo.lst_lc_pro_coordenada = new List<lc_pro_coordenada>();
                    dpFechaLabel.Text = fecha.ToString("dd/MM/yyyy");
                    //VarGlobal.pro_hallazgo.hora = hora.ToString();
                    pkHoraLabel.Text = hora.Substring(0, 2);
                    pkMinutoLabel.Text = hora.Substring(3, 2);
                    VarGlobal.pro_hallazgo.nom_hallazgoclase = pkHallazgoLabel.Text;
                    lbl_reportado.Text = VarGlobal.nom_personal;
                    lbl_cod_personal.Text = VarGlobal.cod_personal;
                    if (string.IsNullOrEmpty(VarGlobal.pro_hallazgo.nom_tblocurrenciatipo)) { btn_ocurrencia.IsEnabled = false; }
                    if (string.IsNullOrEmpty(VarGlobal.pro_hallazgo.nom_tipoubicacion)) { btn_ubicacion.IsEnabled = false; }
                    s_comando = "N";
                    AsignarCoordenada();
                    break;
                case "B":
                    dpFechaLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(0, 10);
                    pkHoraLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(11, 2);
                    pkMinutoLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(14, 2);
                    if (string.IsNullOrEmpty(VarGlobal.pro_hallazgo.nom_tblocurrenciatipo)) { btn_ocurrencia.IsEnabled = false; } else { btn_ocurrencia.IsEnabled = true; }
                    if (string.IsNullOrEmpty(VarGlobal.pro_hallazgo.nom_tipoubicacion)) { btn_ubicacion.IsEnabled = false; } else { btn_ubicacion.IsEnabled = true; }
                    edOcurrio.Text = VarGlobal.pro_hallazgo.des_hallazgo;
                    edQheHacer.Text = VarGlobal.pro_hallazgo.des_tarea;
                    pkSistemaLabel.Text = VarGlobal.pro_hallazgo.nom_sisgestion;
                    pkHallazgoLabel.Text = VarGlobal.pro_hallazgo.nom_hallazgoclase;
                    pkNivelRiesgoLabel.Text = VarGlobal.pro_hallazgo.nom_tblnivelriesgo;
                    pkTipoLabel.Text = VarGlobal.pro_hallazgo.nom_tblocurrenciatipo;
                    pkUbicacionLabel.Text = VarGlobal.pro_hallazgo.nom_tipoubicacion;
                    lbl_ocurrencia.Text = VarGlobal.pro_hallazgo.nom_ocurrencia;
                    if (string.IsNullOrEmpty(VarGlobal.pro_hallazgo.nom_personal))
                    {
                        lbl_reportado.Text = VarGlobal.nom_personal;
                        lbl_cod_personal.Text = VarGlobal.cod_personal;
                    }
                    else
                    {
                        lbl_reportado.Text = VarGlobal.pro_hallazgo.nom_personal;
                        lbl_cod_personal.Text = VarGlobal.pro_hallazgo.cod_personal;
                    }
                    lbl_responsable.Text = VarGlobal.pro_hallazgo.nom_eje_personal;
                    MostrarUbicacion();
                    AsignarCoordenadaB();
                    if (s_comando == "M")
                    {
                        sl_responsable.IsVisible = false;
                        sl_Quehacer.IsVisible = false;
                    }
                    break;
                case "M":
                    dpFechaLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(0, 10);
                    pkHoraLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(11, 2);
                    pkMinutoLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(14, 2);
                    edOcurrio.Text = VarGlobal.pro_hallazgo.des_hallazgo;
                    lbl_ocurrencia.Text = VarGlobal.pro_hallazgo.nom_ocurrencia;
                    lbl_reportado.Text = VarGlobal.pro_hallazgo.nom_personal;
                    //lbl_responsable.Text = VarGlobal.pro_tarea.nom_eje_personal;
                    lbl_cod_personal.Text = VarGlobal.pro_hallazgo.cod_personal;
                    pkSistemaLabel.Text = VarGlobal.pro_hallazgo.nom_sisgestion;
                    pkHallazgoLabel.Text = VarGlobal.pro_hallazgo.nom_hallazgoclase;
                    pkNivelRiesgoLabel.Text = VarGlobal.pro_hallazgo.nom_tblnivelriesgo;
                    pkTipoLabel.Text = VarGlobal.pro_hallazgo.nom_tblocurrenciatipo;
                    pkUbicacionLabel.Text = VarGlobal.pro_hallazgo.nom_tipoubicacion;
                    sl_responsable.IsVisible = false;
                    sl_Quehacer.IsVisible = false;
                    s_comando = "M";
                    MostrarUbicacion();
                    AsignarCoordenada();
                    break;
            }
        }

        private void MostrarPins()
        {
            var lista = VarGlobal.pro_hallazgo.lst_lc_pro_coordenada; //VarGlobal.pro_hallazgo.lst_coordenada;
            for (int i = 0; i < lista.Count; i++)
            {
                var position = new Position(Convert.ToDouble(lista[i].lat_coordenada), Convert.ToDouble(lista[i].lon_coordenada));
                var pin = new Pin { Type = PinType.Place, Position = position, Label = lista[i].com_coordenada };
                Mapa.Pins.Add(pin);
            }
            count_coord = VarGlobal.pro_hallazgo.lst_lc_pro_coordenada.Count(); //VarGlobal.pro_hallazgo.lst_coordenada.Count();
            lbl_coordenada.Text = "Coordenadas " + " ( " + count_coord + " ) ";
        }

        private void CargarCoordenadas()
        {
            lc_pro_coordenada_Data o_Data = new lc_pro_coordenada_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                && x.cod_unidad == VarGlobal.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo
                                                && x.cod_modulo == VarGlobal.cod_modulo).ToList();
            VarGlobal.pro_hallazgo.lst_lc_pro_coordenada = lista;
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
            VarGlobal.pro_hallazgo.lst_lc_pro_coordenada.Add(new lc_pro_coordenada()
            {
                cod_empresa = VarGlobal.cod_empresa,
                cod_unidad = VarGlobal.cod_unidad,
                cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo,
                cod_modulo = VarGlobal.pro_hallazgo.cod_modulo,
                num_coordenada = 0,
                lat_coordenada = VarGlobal.pro_hallazgo.latitud,
                lon_coordenada = VarGlobal.pro_hallazgo.longitud,
                com_coordenada = s_creado,
                sincronizado = false,
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip,
                estado = "A",
                comando = "INS",
            });
            MostrarPins();
        }

        private async void AsignarCoordenada()
        {
            var position = await CrossGeolocator.Current.GetPositionAsync();
            VarGlobal.pro_hallazgo.latitud = Convert.ToDecimal(position.Latitude);
            VarGlobal.pro_hallazgo.longitud = Convert.ToDecimal(position.Longitude);

            var position1 = new Position(position.Latitude, position.Longitude);

            //  Asignar Posicion.
            MapSpan mp = MapSpan.FromCenterAndRadius(position1, Distance.FromKilometers(1));
            Device.BeginInvokeOnMainThread(() =>
            {
                Mapa.MoveToRegion(mp);
                InsertarCoordenada();
            });

        }

        private async void AsignarCoordenadaB()
        {
            var position = await CrossGeolocator.Current.GetPositionAsync();
            var position1 = new Position(position.Latitude, position.Longitude);
            //var pin = new Pin { Type = PinType.Place, Position = position1, Label = "Soy un Pin" };

            //  Asignar Posicion.
            MapSpan mp = MapSpan.FromCenterAndRadius(position1, Distance.FromKilometers(1));
            Mapa.MoveToRegion(mp);
            MostrarPins();
        }

        private void CargarVariables()
        {
            VarGlobal.pro_hallazgo.cod_empresa = VarGlobal.cod_empresa;
            VarGlobal.pro_hallazgo.cod_unidad = VarGlobal.cod_unidad;
            VarGlobal.pro_hallazgo.cod_usuario = VarGlobal.cod_usuario;
        }

        private void CargarEvidencias()
        {
            lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_hallazgo.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_hallazgo.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo).ToList();
            VarGlobal.pro_hallazgo.lst_lc_pro_evidencia = lista;
        }

        private void MostrarEvidencias()
        {
            List<lc_pro_evidencia> lista = VarGlobal.pro_hallazgo.lst_lc_pro_evidencia;
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
                tit_mensaje = "Observación Preventiva",
                tex_mensaje = "¿Desea Eliminar Foto?"
            });
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                var s_cod_referencia = ((Button)sender).CommandParameter;
                var lista = VarGlobal.pro_hallazgo.lst_lc_pro_evidencia;
                for (int i = 0; i < lista.Count; i++)
                {
                    int s_cod = lista[i].num_evidencia;
                    if (s_cod == Convert.ToInt32(s_cod_referencia))
                    {
                        lista.RemoveAt(i);
                        i--;
                    }
                }
                VarGlobal.pro_hallazgo.lst_lc_pro_evidencia = lista;
                stInf.Children.Remove(stInf);
                await Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
            }
            await Navigation.RemovePopupPageAsync(loadingPage);

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void MostrarUbicacion()
        {
            ubicacion = VarGlobal.pro_hallazgo.cod_tipoubicacion;
            switch (VarGlobal.pro_hallazgo.cod_tipoubicacion)
            {
                case "E":
                    lbl_ubicacion.Text = VarGlobal.pro_hallazgo.nom_equipo;
                    VarGlobal.pro_hallazgo.cod_labor = "";
                    VarGlobal.pro_hallazgo.nom_labor = "";
                    VarGlobal.pro_hallazgo.cod_lugar = "";
                    VarGlobal.pro_hallazgo.nom_lugar = "";
                    break;
                case "I":
                    lbl_ubicacion.Text = VarGlobal.pro_hallazgo.nom_labor;
                    VarGlobal.pro_hallazgo.cod_lugar = "";
                    VarGlobal.pro_hallazgo.nom_lugar = "";
                    VarGlobal.pro_hallazgo.cod_equipo = "";
                    VarGlobal.pro_hallazgo.nom_equipo = "";
                    break;
                case "S":
                    lbl_ubicacion.Text = VarGlobal.pro_hallazgo.nom_lugar;
                    VarGlobal.pro_hallazgo.cod_labor = "";
                    VarGlobal.pro_hallazgo.nom_labor = "";
                    VarGlobal.pro_hallazgo.cod_equipo = "";
                    VarGlobal.pro_hallazgo.nom_equipo = "";
                    break;
            }
        }

        private void MostrarListaInfractores()
        {
            List<lc_pro_participante> lista = VarGlobal.pro_hallazgo.lst_lc_pro_participante;
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

        private void CargarInfractores()
        {
            lc_pro_participante_Data o_Data = new lc_pro_participante_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_hallazgo.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_veoregistro.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_hallazgo.cod_hallazgo
                                                && x.tip_participante == "I").ToList();
            VarGlobal.pro_hallazgo.lst_lc_pro_participante = lista;
        }


        #region Seleccionar Picker

        private async void pkTipoLabel_Tapped(object sender, EventArgs e)
        {
            lc_aux_tbldetalle_Data o_Data = new lc_aux_tbldetalle_Data();
            var lst_tbldetalle = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                        && x.cod_tblgrupo == "001").ToList();

            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "TIPO";
            for (int i = 0; i < lst_tbldetalle.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_tbldetalle[i].cod_tbldetalle,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_tbldetalle[i].nom_tbldetalle
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_hallazgo.cod_tblocurrenciatipo = result;
                VarGlobal.pro_hallazgo.nom_tblocurrenciatipo = lst_tbldetalle.Where(x => x.cod_tbldetalle == result).FirstOrDefault().nom_tbldetalle.ToString();

                lbl_ocurrencia.Text = "";
                await Navigation.PushAsync(new pg_ocurrencia(VarGlobal.pro_hallazgo.cod_tblocurrenciatipo)
                {
                    Title = "Seleccionar"
                });

                pkTipoLabel.Text = VarGlobal.pro_hallazgo.nom_tblocurrenciatipo;
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
                VarGlobal.pro_hallazgo.cod_tipoubicacion = result;
                VarGlobal.pro_hallazgo.nom_tipoubicacion = lst_ubicacion.Where(x => x.cod_tipoubicacion == result).FirstOrDefault().nom_tipoubicacion.ToString();

                VarGlobal.cod_modulo_ret = "HL";
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
                pkUbicacionLabel.Text = VarGlobal.pro_hallazgo.nom_tipoubicacion;
            }
            //pkUbicacion.Focus();
        }

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
                VarGlobal.pro_hallazgo.cod_sisgestion = result;
                VarGlobal.pro_hallazgo.nom_sisgestion = lst_sisgestion.Where(x => x.cod_sisgestion == result).FirstOrDefault().nom_sisgestion.ToString();

                pkSistemaLabel.Text = VarGlobal.pro_hallazgo.nom_sisgestion;
            }
            //pkSistema.Focus();
        }


        private async void pkHallazgoLabel_Tapped(object sender, EventArgs e)
        {
            lc_glb_hallazgoclase_Data o_Data = new lc_glb_hallazgoclase_Data();
            var lst_hallazgoclase = o_Data.Listar();

            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "CLASES";
            for (int i = 0; i < lst_hallazgoclase.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_hallazgoclase[i].cod_hallazgoclase,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_hallazgoclase[i].nom_hallazgoclase
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_hallazgo.cod_hallazgoclase = result;
                VarGlobal.pro_hallazgo.nom_hallazgoclase = lst_hallazgoclase.Where(x => x.cod_hallazgoclase == result).FirstOrDefault().nom_hallazgoclase.ToString();

                pkHallazgoLabel.Text = VarGlobal.pro_hallazgo.nom_hallazgoclase;
            }
        }

        private async void pkNivelRiesgoLabel_Tapped(object sender, EventArgs e)
        {
            lc_aux_tbldetalle_Data o_Data = new lc_aux_tbldetalle_Data();
            var lst_tbldetalle = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                          && x.cod_tblgrupo == "003").ToList();

            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "NIVEL";
            for (int i = 0; i < lst_tbldetalle.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = lst_tbldetalle[i].cod_tbldetalle,
                    ico_mensaje = "fa-check",
                    tex_mensaje = lst_tbldetalle[i].nom_tbldetalle
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_hallazgo.cod_tblnivelriesgo = result;
                VarGlobal.pro_hallazgo.nom_tblnivelriesgo = lst_tbldetalle.Where(x => x.cod_tbldetalle == result).FirstOrDefault().nom_tbldetalle.ToString();

                pkNivelRiesgoLabel.Text = VarGlobal.pro_hallazgo.nom_tblnivelriesgo;
            }
        }

        #endregion

        #region Button

        private async void btn_grabar_Clicked(object sender, EventArgs e)
        {

            if (!ValidarObservacion())
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "INF",
                    tit_mensaje = "Observación Preventiva",
                    tex_mensaje = "¿Desea Grabar Registro?"
                });
                var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
                await Navigation.PushPopupAsync(loadingPage);

                if (result)
                {
                    lc_pro_hallazgo_Data o_Data = new lc_pro_hallazgo_Data();
                    //  Generar Codigo, solo si viene VACIO O NULO
                    if (string.IsNullOrEmpty(VarGlobal.pro_hallazgo.cod_hallazgo))
                    {
                        var conteo = (o_Data.Listar().Count) + 1;
                        var año = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(8, 2);
                        var mes = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(3, 2);
                        string s_codigo = "HL" + año + mes + "-" + conteo;

                        VarGlobal.pro_hallazgo.cod_hallazgo = s_codigo;
                        VarGlobal.pro_hallazgo.cod_personal = lbl_cod_personal.Text;
                        VarGlobal.pro_hallazgo.nom_personal = lbl_reportado.Text;
                        GrabarTarea();
                    }
                    VarGlobal.pro_hallazgo.sincronizado = false;
                    o_Data.Modificar(VarGlobal.pro_hallazgo);
                    if (VarGlobal.cod_modulo == "OB")
                    {
                        await Navigation.PushModalAsync(new MasterDetailPage1(VarGlobal.ret_hallazgo_hijo));
                    }
                    else
                    {
                        Retornar();
                    }
                }
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        private void GrabarTarea()
        {
            DateTime fecha = DateTime.Now;
            lc_aux_estado_Data o_Data_Est = new lc_aux_estado_Data();
            lc_pro_tarea ent_tarea = new lc_pro_tarea();
            lc_aux_estado ent_estado = o_Data_Est.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                   && x.cod_modulo == "TR"
                                                                   && x.cod_estado == "01").FirstOrDefault();

            ent_tarea = new lc_pro_tarea()
            {
                cod_empresa = VarGlobal.pro_hallazgo.cod_empresa,
                cod_unidad = VarGlobal.pro_hallazgo.cod_unidad,
                cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo,
                des_origen = VarGlobal.pro_hallazgo.nom_hallazgoclase,
                cod_modulo = "HL",
                cod_estado = ent_estado.cod_estado,
                nom_estado = ent_estado.nom_estado,
                ini_tarea = fecha.ToString("dd/MM/yyyy"),
                fin_tarea = fecha.AddDays(4).ToString("dd/MM/yyyy"),
                sol_personal = VarGlobal.pro_hallazgo.cod_personal,
                eje_personal = VarGlobal.pro_hallazgo.eje_personal,
                nom_eje_personal = VarGlobal.pro_hallazgo.nom_eje_personal,
                nom_sol_personal = VarGlobal.pro_hallazgo.nom_personal,
                des_tarea = VarGlobal.pro_hallazgo.des_tarea,
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
                ver_opcion = ent_estado.ver_opcion,
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip,
                estado = "A",
                comando = "INS"
            };

            lc_pro_tarea_Data o_Data = new lc_pro_tarea_Data();
            var conteo = (o_Data.Listar().Count) + 1;
            var año = ent_tarea.ini_tarea.Substring(8, 2);
            var mes = ent_tarea.ini_tarea.Substring(3, 2);
            ent_tarea.cod_tarea = "TR" + año + mes + "-" + conteo;
            ent_tarea.sincronizado = false;
            o_Data.Modificar(ent_tarea);
        }

        private void btn_ocurrencia_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pg_ocurrencia(VarGlobal.pro_hallazgo.cod_tblocurrenciatipo) { Title = "Seleccionar" });
        }

        private void btn_reportado_Clicked(object sender, EventArgs e)
        {
            var btn = ((Button)sender).CommandParameter;
            switch (btn)
            {
                case "Rep":
                    Navigation.PushAsync(new pg_personal("OB", "Rep") { Title = "Seleccionar" });
                    break;
                case "Inf":
                    Navigation.PushAsync(new pg_personal("OB", "Inf") { Title = "Seleccionar" });
                    break;
                case "Ejec":
                    Navigation.PushAsync(new pg_personal("OB", "Ejec") { Title = "Seleccionar" });
                    break;
            }
        }

        private void btn_ubicacion_Clicked(object sender, EventArgs e)
        {
            VarGlobal.cod_modulo_ret = "HL";
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

        #endregion

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            VarGlobal.pro_hallazgo.des_hallazgo = texto;
        }

        private void edQheHacer_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = ((Editor)sender).Text;
            VarGlobal.pro_hallazgo.des_tarea = texto;
        }

        private bool ValidarObservacion()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (s_comando == "N")
            {
                if (string.IsNullOrEmpty(lbl_responsable.Text) && !b_Error)
                {
                    b_Error = true;
                    s_Mensaje = "Seleccionar Responsable...";
                }

                if (string.IsNullOrEmpty(edQheHacer.Text))
                {
                    b_Error = true;
                    s_Mensaje = "Ingresar Que Hacer...";
                }
            }

            if (string.IsNullOrEmpty(edOcurrio.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Que ocurrio...";
            }

            if (string.IsNullOrEmpty(pkSistemaLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Sistema...";
            }

            if (string.IsNullOrEmpty(pkHallazgoLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Hallazgo Clase...";
            }

            if (string.IsNullOrEmpty(pkNivelRiesgoLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Nivel Riesgo...";
            }

            if (string.IsNullOrEmpty(pkTipoLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Tipo...";
            }

            if (string.IsNullOrEmpty(lbl_ocurrencia.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Ocurrencia...";
            }

            if (string.IsNullOrEmpty(lbl_reportado.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Reportante...";
            }

            if (string.IsNullOrEmpty(pkUbicacionLabel.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Ubicación...";
            }

            if (string.IsNullOrEmpty(lbl_ubicacion.Text) && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Seleccionar Detalle ubicación...";
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

        void btnEliInfractorClicked(object sender, EventArgs e)
        {
            var s_cod_personal = ((Button)sender).CommandParameter;
            var lista = VarGlobal.pro_hallazgo.lst_lc_pro_participante;
            for (int i = 0; i < lista.Count; i++)
            {
                string s_cod = lista[i].cod_personal;
                if (s_cod == s_cod_personal.ToString())
                {
                    lista.RemoveAt(i);
                    i--;
                }
            }
            VarGlobal.pro_hallazgo.lst_lc_pro_participante = lista;
            stInf.Children.Remove(stInf);
            Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
        }

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "WAR",
                tit_mensaje = "Hallazgo",
                tex_mensaje = "¿Seguro que deseas salir?"
            });
            var result = await popupAlert.Show();
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                if (VarGlobal.cod_modulo == "OB")
                {
                    await Navigation.PushModalAsync(new MasterDetailPage1(VarGlobal.ret_hallazgo_hijo));
                }
                else
                {
                    Retornar();
                }
            }
            await Navigation.RemovePopupPageAsync(loadingPage);
        }

        private async void Retornar()
        {
            string s_retorno = VarGlobal.ret_hallazgo_hijo;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));
        }

        private void dpFechaLabel_Tapped(object sender, EventArgs e)
        {
            dpFecha.Focus();
        }

        private void dpFecha_DateSelected(object sender, DateChangedEventArgs e)
        {
            var fecha = ((DatePicker)sender).Date;
            dpFechaLabel.Text = fecha.ToString("dd/MM/yyyy");
        }

        private void pkHora_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ((Picker)sender).SelectedItem;
            pkHoraLabel.Text = item.ToString();

            VarGlobal.pro_hallazgo.hora = pkHoraLabel.Text + ":" + pkMinutoLabel.Text;
        }

        private void pkHoraLabel_Tapped(object sender, EventArgs e)
        {
            pkHora.Focus();
        }

        private void pkMinuto_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ((Picker)sender).SelectedItem;
            pkMinutoLabel.Text = item.ToString();

            VarGlobal.pro_hallazgo.hora = pkHoraLabel.Text + ":" + pkMinutoLabel.Text;
        }

        private void pkMinutoLabel_Tapped(object sender, EventArgs e)
        {
            pkMinuto.Focus();
        }

        private void IconButton_Clicked(object sender, EventArgs e)
        {
            VarGlobal.cod_modulo_ret = "HL";
            Navigation.PushAsync(new pg_evidencia() { Title = "Agregar Evidencia" });
        }

        private void img_foto_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}
