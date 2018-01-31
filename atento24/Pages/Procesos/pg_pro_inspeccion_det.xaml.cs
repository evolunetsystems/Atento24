using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Recursos;
using Plugin.Iconize;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_inspeccion_det : ContentPage
    {
        public pg_pro_inspeccion_det()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            MostrarDetalle();
            CargarParticipantes();
            MostrarParticipantes();
        }

        private void MostrarParticipantes()
        {
            List<lc_pro_participante> lista = VarGlobal.pro_inspeccion.lst_lc_pro_participante;
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
                    stInf.Children.Add(stCon);
                }
            }
        }

        private void MostrarDetalle()
        {
            dpFechaLabel.Text = VarGlobal.pro_inspeccion.fec_inspeccion;
            pkTipoLabel.Text = VarGlobal.pro_inspeccion.nom_inspecciontipo;
            edTitulo.Text = VarGlobal.pro_inspeccion.tit_inspeccion;
            edObjetivo.Text = VarGlobal.pro_inspeccion.obj_inspeccion;
            lbl_reportado.Text = VarGlobal.pro_inspeccion.nom_personal;
            pkSistemaLabel.Text = VarGlobal.pro_inspeccion.nom_sisgestion;
            pkPreLabel.Text = VarGlobal.pro_inspeccion.nom_inspeccionpre;
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

        private async void btn_salir_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_inspeccion_qry"));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }
    }
}
