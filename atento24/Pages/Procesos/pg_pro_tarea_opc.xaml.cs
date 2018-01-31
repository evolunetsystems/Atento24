using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_tarea_opc : ContentPage
    {
        public pg_pro_tarea_opc()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            LimpiarVariables();
            ListarOpciones();
        }

        private void ListarOpciones()
        {
            lc_pro_tarea_Data o_Data_tar = new lc_pro_tarea_Data();
            List<lc_pro_tarea> lst_tarea = o_Data_tar.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                               && x.cod_unidad == VarGlobal.cod_unidad).ToList();

            int total = lst_tarea.Where(x => x.eje_personal == VarGlobal.cod_personal && x.ver_opcion.Contains("A")).ToList().Count;

            List<ent_opcion> lst_opcion = new List<ent_opcion>();
            lst_opcion.Add(new ent_opcion
            {
                cod_opcion = "01",
                nom_opcion = "Tareas por Atender",
                ver_opcion = "A",
                can_opcion = lst_tarea.Where(x => x.eje_personal == VarGlobal.cod_personal && x.ver_opcion.Contains("A")).ToList().Count
            });
            lst_opcion.Add(new ent_opcion
            {
                cod_opcion = "02",
                nom_opcion = "Tareas por Verificar",
                ver_opcion = "V",
                can_opcion = lst_tarea.Where(x => x.sol_personal == VarGlobal.cod_personal && x.ver_opcion.Contains("V")).ToList().Count
            });
            lst_opcion.Add(new ent_opcion
            {
                cod_opcion = "03",
                nom_opcion = "Favoritos",
                ver_opcion = "X",
                can_opcion = lst_tarea.Where(x => x.sol_personal != VarGlobal.cod_personal
                                         && x.eje_personal != VarGlobal.cod_personal
                                         && x.ver_opcion.Contains("X")).ToList().Count
            });
            lst_opcion.Add(new ent_opcion
            {
                cod_opcion = "04",
                nom_opcion = "Tareas Solicitadas",
                ver_opcion = "S",
                can_opcion = lst_tarea.Where(x => x.usu_crea == VarGlobal.cod_usuario).ToList().Count
            });

            AccionListView.ItemsSource = lst_opcion;
        }

        public class ent_opcion
        {
            public string cod_opcion { get; set; }
            public string nom_opcion { get; set; }
            public int can_opcion { get; set; }
            public string ver_opcion { get; set; }
        }

        private void LimpiarVariables()
        {
            VarGlobal.ver_opcion = "";
            VarGlobal.cod_tarea = "";
            VarGlobal.des_tarea = "";
            VarGlobal.por_avance = 0;
        }

        private async void SeleccionarAccion(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ent_opcion;
            int i_cantidad = item.can_opcion;

            if (i_cantidad > 0)
            {
                Content.IsEnabled = false;
                var loadingPage = new pg_Loading();
                await Navigation.PushPopupAsync(loadingPage);


                VarGlobal.ver_opcion = item.ver_opcion;
                VarGlobal.cod_modulo = "";
                VarGlobal.cod_referencia = "";
                if (item == null)
                    return;
                VarGlobal.pro_tarea = new lc_pro_tarea();
                VarGlobal.ret_tarea_padre = "pg_pro_tarea_opc";
                VarGlobal.pro_tarea.titulo = item.nom_opcion;
                VarGlobal.ret_titulo = item.nom_opcion;

                switch (item.cod_opcion)
                {
                    case "01":
                        //PorAtender                    
                        await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_qry"));
                        break;
                    case "02":
                        //PorVerificar
                        await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_qry"));
                        break;
                    case "03":
                        //Favorito
                        break;
                    case "04":
                        //Favorito
                        await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_tarea_qry"));
                        break;
                }

                await Navigation.RemovePopupPageAsync(loadingPage);
                Content.IsEnabled = true;
            }
        }

        private void AccionListView_Refreshing(object sender, EventArgs e)
        {
            ListarOpciones();
            AccionListView.IsRefreshing = false;
        }

        private void btnNue_Clicked(object sender, EventArgs e)
        {
            lc_aux_estado_Data o_lc_aux_estado_Data = new lc_aux_estado_Data();
            lc_aux_estado o_lc_aux_estado = o_lc_aux_estado_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                    && x.cod_estado == "01").ToList().FirstOrDefault();

            VarGlobal.pro_tarea = new lc_pro_tarea();
            VarGlobal.pro_tarea.cod_tarea = "";
            VarGlobal.pro_tarea.cod_referencia = "";
            VarGlobal.pro_tarea.cod_modulo_2do = "";
            VarGlobal.pro_tarea.cod_modulo = "TR";
            VarGlobal.pro_tarea.cod_estado = o_lc_aux_estado.cod_estado;
            VarGlobal.pro_tarea.nom_estado = o_lc_aux_estado.nom_estado;
            VarGlobal.pro_tarea.ver_opcion = o_lc_aux_estado.ver_opcion;
            VarGlobal.pro_tarea.ret_titulo = "Nueva Tarea";
            VarGlobal.ret_tarea_hijo = "pg_pro_tarea_opc";
            VarGlobal.pro_tarea.usuario = VarGlobal.cod_usuario;
            VarGlobal.pro_tarea.usu_crea = VarGlobal.cod_usuario;
            VarGlobal.pro_tarea.fec_crea = DateTime.Now.ToString("dd/MM/yyyy");
            VarGlobal.pro_tarea.ip = VarGlobal.ip;
            VarGlobal.pro_tarea.estado = "A";
            VarGlobal.pro_tarea.comando = "INS";

            Navigation.PushAsync(new pg_pro_tarea_mnt("N") { Title = VarGlobal.pro_tarea.ret_titulo });
        }
    }
}
