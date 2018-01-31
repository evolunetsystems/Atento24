using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Recursos;
using Plugin.Iconize;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_incidente_det : ContentPage
    {
        public pg_pro_incidente_det()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            MostrarDetalle();
            MostrarListaInfractores();
            MostrarImagen();
        }

        private void MostrarListaInfractores()
        {
            lc_pro_incidente_personal_Data o_Data = new lc_pro_incidente_personal_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_incidente.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_incidente.cod_unidad
                                                && x.cod_incidente == VarGlobal.pro_incidente.cod_incidente).ToList();
            VarGlobal.pro_incidente.lst_lc_pro_incidente_personal = lista;
            //List<lc_pro_incidente_personal> lista = VarGlobal.pro_incidente.lst_lc_pro_incidente_personal;
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
            lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.pro_incidente.cod_empresa
                                                && x.cod_unidad == VarGlobal.pro_incidente.cod_unidad
                                                && x.cod_referencia == VarGlobal.pro_incidente.cod_incidente).ToList();
            VarGlobal.pro_incidente.lst_lc_pro_evidencia = lista;
            //List<lc_pro_evidencia> lista = VarGlobal.pro_incidente.lst_lc_pro_evidencia;
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
            dpFechaLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(0, 10);
            pkHoraLabel.Text = VarGlobal.pro_incidente.fec_incidente.Substring(11, 5);
            lbl_des_incidente.Text = VarGlobal.pro_incidente.des_incidente;
            pkSistemaLabel.Text = VarGlobal.pro_incidente.nom_sisgestion;
            pkRealLabel.Text = VarGlobal.pro_incidente.nom_severidadreal;
            pkPoteLabel.Text = VarGlobal.pro_incidente.nom_severidadpot;
            lbl_reportado.Text = VarGlobal.pro_incidente.nom_personal;
            pkUbicacionLabel.Text = VarGlobal.pro_incidente.nom_tipoubicacion;
            switch (VarGlobal.pro_incidente.cod_tipoubicacion)
            {
                case "E":
                    lbl_ubicacion.Text = VarGlobal.pro_incidente.nom_equipo;
                    break;
                case "I":
                    lbl_ubicacion.Text = VarGlobal.pro_incidente.nom_labor;
                    break;
                case "S":
                    lbl_ubicacion.Text = VarGlobal.pro_incidente.nom_lugar;
                    break;
            }
        }

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            string s_retorno = VarGlobal.pro_incidente.retorno;
            await Navigation.PushModalAsync(new MasterDetailPage1(s_retorno));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }
    }
}
