using System;
using System.Collections.Generic;
using System.IO;
using atento24.Data.ORM;
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
    public partial class pg_pro_hallazgo_det : ContentPage
    {
        public static int count_coord;
        public pg_pro_hallazgo_det()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            MostrarDetalle();
            MostrarListaInfractores();
            MostrarImagen();
            MostrarPins();
            lbl_coordenada.Text = "Coordenadas " + " ( " + count_coord + " ) ";
        }

        private async void AsignarCoordenadaB()
        {
            var position = await CrossGeolocator.Current.GetPositionAsync();
            var position1 = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            var pin = new Pin { Type = PinType.Place, Position = position1, Label = "Soy un Pin" };

            //  Asignar Posicion.
            MapSpan mp = MapSpan.FromCenterAndRadius(position1, Distance.FromKilometers(1));
            Device.BeginInvokeOnMainThread(() =>
            {
                Mapa.MoveToRegion(mp);
                MostrarPins();
            });

        }

        private void MostrarPins()
        {
            var lista = VarGlobal.pro_hallazgo.lst_lc_pro_coordenada;
            for (int i = 0; i < lista.Count; i++)
            {
                var position = new Xamarin.Forms.Maps.Position(Convert.ToDouble(lista[i].lat_coordenada), Convert.ToDouble(lista[i].lon_coordenada));
                var pin = new Pin { Type = PinType.Place, Position = position, Label = lista[i].com_coordenada };

                MapSpan mp = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1));
                Mapa.MoveToRegion(mp);
                Mapa.Pins.Add(pin);
            }
            count_coord = lista.Count;
        }

        private void MostrarListaInfractores()
        {
            List<lc_pro_participante> lista = VarGlobal.pro_hallazgo.lst_lc_pro_participante;
            if (lista != null)
            {
                //  Creando StackLayout Contenedor.
                for (int i = 0; i < lista.Count; i++)
                {
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
                    //stCon.Children.Add(btnEli);
                    stInf.Children.Add(stCon);
                }
            }
        }

        private void MostrarImagen()
        {
            List<lc_pro_evidencia> lista = VarGlobal.pro_hallazgo.lst_lc_pro_evidencia;
            if (lista != null)
            {
                //  Creando StackLayout Contenedor.
                for (int i = 0; i < lista.Count; i++)
                {
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

                    //stCom.Children.Add(btnEli);
                    stCom.Children.Add(lblCom);
                    stCab.Children.Add(imgFoto);
                    stCab.Children.Add(stCom);
                    stDet.Children.Add(stCab);
                }
            }
        }

        private void MostrarDetalle()
        {
            dpFechaLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(0, 10);
            pkHoraLabel.Text = VarGlobal.pro_hallazgo.fec_hallazgo.Substring(11, 5);
            edOcurrio.Text = VarGlobal.pro_hallazgo.des_hallazgo;
            pkSistemaLabel.Text = VarGlobal.pro_hallazgo.nom_sisgestion;
            pkHallazgoLabel.Text = VarGlobal.pro_hallazgo.nom_hallazgoclase;
            pkNivelRiesgoLabel.Text = VarGlobal.pro_hallazgo.nom_tblnivelriesgo;
            pkTipoLabel.Text = VarGlobal.pro_hallazgo.nom_tblocurrenciatipo;
            lbl_ocurrencia.Text = VarGlobal.pro_hallazgo.nom_ocurrencia;
            lbl_reportado.Text = VarGlobal.pro_hallazgo.nom_personal;
            pkUbicacionLabel.Text = VarGlobal.pro_hallazgo.nom_tipoubicacion;
            switch (VarGlobal.pro_hallazgo.cod_tipoubicacion)
            {
                case "E":
                    lbl_ubicacion.Text = VarGlobal.pro_hallazgo.nom_equipo;
                    break;
                case "I":
                    lbl_ubicacion.Text = VarGlobal.pro_hallazgo.nom_labor;
                    break;
                case "S":
                    lbl_ubicacion.Text = VarGlobal.pro_hallazgo.nom_lugar;
                    break;
            }
        }

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            string s_retorno = VarGlobal.ret_hallazgo_hijo;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }
    }
}
