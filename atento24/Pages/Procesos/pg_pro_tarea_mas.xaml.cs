using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Recursos;
using Plugin.Iconize;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_tarea_mas : ContentPage
    {
        public pg_pro_tarea_mas()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            CargarAccion();
            CargarAvances();
        }

        private void CargarAccion()
        {
            lbl_cod_tarea.Text = VarGlobal.pro_tarea.cod_tarea;
            lbl_nom_origen.Text = VarGlobal.pro_tarea.des_origen;
            lbl_des_tarea.Text = VarGlobal.pro_tarea.des_tarea;
            dpDesde.Text = VarGlobal.pro_tarea.ini_tarea;
            dpHasta.Text = VarGlobal.pro_tarea.fin_tarea;
            lbl_nom_sol_personal.Text = VarGlobal.pro_tarea.nom_sol_personal;
            lbl_nom_eje_personal.Text = VarGlobal.pro_tarea.nom_eje_personal;
            lbl_nom_ubicacion.Text = VarGlobal.pro_tarea.nom_ubicacion;
            lbl_nom_estado.Text = VarGlobal.pro_tarea.nom_estado;
        }

        private void CargarAvances()
        {
            lc_pro_avance_Data o_Data = new lc_pro_avance_Data();
            var lista = o_Data.Listar();
            List<lc_pro_avance> lst_avance = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                        && x.cod_unidad == VarGlobal.cod_unidad
                                                                        && x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).OrderByDescending(x => x.num_avance).ToList();

            CargarEvidencias();

            for (int i = 0; i < lst_avance.Count; i++)
            {
                int i_num_etapa = lst_avance[i].num_avance;
                var ent_evidencia = VarGlobal.pro_tarea.lst_lc_pro_evidencia.Where(x => x.num_etapa == i_num_etapa).FirstOrDefault();

                var tipo = lst_avance[i].tip_avance;
                if (tipo == "A")
                {
                    lst_avance[i].vpor_avance = lst_avance[i].por_avance.ToString().Trim() + "%";
                }
                else
                {
                    lst_avance[i].vpor_avance = "";
                }

                //  CREANDO STACKLAYOUT CONTENEDOR.
                StackLayout stCon = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 1),
                    Spacing = 0,
                    Padding = 5,
                    BackgroundColor = Color.White,
                    Children = {
                                new IconImage{
                                Icon="fa-user-circle",
                                IconColor = Color.Black,
                                VerticalOptions = LayoutOptions.Start,
                                Margin = new Thickness(0, 0, 5, 0),
                                IconSize = 20,
                                WidthRequest = 30,
                                HeightRequest = 30
                            }
                        }
                };

                StackLayout stHij = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Spacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                            new Label {
                                Text = lst_avance[i].nom_personal,
                                TextColor = Color.Black,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 12
                            },
                            new Label {
                                Text = lst_avance[i].fec_avance, Margin = 0,
                                FontSize = 11,
                                TextColor = Color.Gray
                            },
                            new Label {
                                Text = lst_avance[i].vpor_avance.ToString().Trim() + " de Avance.",
                                FontSize = 11,
                                TextColor = Color.FromHex("#5F6A6A"),
                                IsVisible = (lst_avance[i].tip_avance == "A")
                            },
                            new Label {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Text = lst_avance[i].des_avance.Trim(),
                                Margin = new Thickness(0, 5, 0, 0),
                                FontSize = 11,
                                TextColor = Color.FromHex("#2E2E2E")
                            }
                        }
                };

                // CREANDO IMAGEN
                if (ent_evidencia != null)
                {
                    Stream stream = new MemoryStream(ent_evidencia.dat_evidencia);
                    Image imgFoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return stream; }),
                        HeightRequest = 150,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    stHij.Children.Add(imgFoto);
                }
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    stCon.Children.Add(stHij);
                //    stPri.Children.Add(stCon);
                //});
                stCon.Children.Add(stHij);
                stPri.Children.Add(stCon);
            }
            //OCULTAR ICONO
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    aiAva.IsVisible = false;
            //});
            aiAva.IsVisible = false;

        }

        private void CargarEvidencias()
        {
            lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
            List<lc_pro_evidencia> lst_evidencia = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                        && x.cod_unidad == VarGlobal.cod_unidad
                                                                        && x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).ToList();
            VarGlobal.pro_tarea.lst_lc_pro_evidencia = lst_evidencia;
        }

        private async void btnsalir_Clicked(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_tarea.ver_opcion = "";
            //var retorno = VarGlobal.pro_tarea.ret_hijo;
            var retorno = VarGlobal.ret_tarea_hijo;
            VarGlobal.pro_tarea.titulo = VarGlobal.ret_titulo;
            await Navigation.PushModalAsync(new MasterDetailPage1(retorno));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }
    }
}
