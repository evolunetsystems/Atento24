using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_empresa : ContentPage
    {
        public pg_empresa()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            //  limpiar Variables
            ListarEmpresas();
        }

        private void ListarEmpresas()
        {
            lc_acc_empresa_Data o_Data = new lc_acc_empresa_Data();
            lc_acc_unidad_Data o_Data_uni = new lc_acc_unidad_Data();
            List<lc_acc_empresa> lst_empresa = o_Data.Listar();
            List<lc_acc_unidad> lst_menu = new List<lc_acc_unidad>();

            for (int i = 0; i < lst_empresa.Count; i++)
            {
                lst_menu.Add(new lc_acc_unidad
                {
                    cod_empresa = lst_empresa[i].cod_empresa,
                    cod_unidad = "",
                    b_visible = false,
                    nom_unidad = lst_empresa[i].nom_empresa,
                    cod_personal = lst_empresa[i].cod_personal,
                    nom_personal = lst_empresa[i].nom_personal,
                    s_color_00 = "#E6E6E6",
                    fontAttribute = FontAttributes.Bold
                });

                List<lc_acc_unidad> lst_unidad = o_Data_uni.Listar().Where(x => x.cod_empresa == lst_empresa[i].cod_empresa).ToList();
                for (int u = 0; u < lst_unidad.Count; u++)
                {
                    lst_menu.Add(new lc_acc_unidad
                    {
                        cod_empresa = lst_unidad[u].cod_empresa,
                        cod_unidad = lst_unidad[u].cod_unidad,
                        b_visible = true,
                        nom_unidad = lst_unidad[u].nom_unidad,
                        cod_personal = lst_empresa[i].cod_personal,
                        nom_personal = lst_empresa[i].nom_personal,
                        s_color_00 = "White",
                        fontAttribute = FontAttributes.None
                    });
                }
            }
            lstEmpresa.ItemsSource = lst_menu;
        }

        private async void SeleccionarEmpresa(object sender, SelectedItemChangedEventArgs e)
        {
            bool b_visible = (e.SelectedItem as lc_acc_unidad).b_visible;
            VarGlobal.cod_empresa = (e.SelectedItem as lc_acc_unidad).cod_empresa;

            if (b_visible)
            {
                Content.IsEnabled = false;
                var loadingPage = new pg_Loading();
                await Navigation.PushPopupAsync(loadingPage);

                //VarGlobal.cod_empresa = (e.SelectedItem as lc_acc_unidad).cod_empresa;
                VarGlobal.cod_unidad = (e.SelectedItem as lc_acc_unidad).cod_unidad;
                VarGlobal.cod_personal = (e.SelectedItem as lc_acc_unidad).cod_personal;
                VarGlobal.nom_personal = (e.SelectedItem as lc_acc_unidad).nom_personal;
                await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_opc"));

                await Navigation.RemovePopupPageAsync(loadingPage);
                Content.IsEnabled = true;
            }


        }

        private void lstEmpresa_Refreshing(object sender, EventArgs e)
        {
            ListarEmpresas();
            lstEmpresa.IsRefreshing = false;
        }
    }
}
