using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Data.StandarDB;
using atento24.Pages.Popup;
using atento24.Pages.Procesos;
using atento24.Recursos;
using Plugin.Iconize;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1 : MasterDetailPage
    {
        public static LocalDB DB { get; private set; }
        public MasterDetailPage1 MainPage { get; private set; }
        public static string ruta { get; set; }

        public lc_pro_hallazgo_Data lc_pro_hallazgo_Data = new lc_pro_hallazgo_Data();
        public lc_pro_tarea_Data lc_pro_tarea_Data = new lc_pro_tarea_Data();
        public lc_pro_veoregistro_Data lc_pro_veoregistro_Data = new lc_pro_veoregistro_Data();
        public lc_pro_inspeccion_Data lc_pro_inspeccion_Data = new lc_pro_inspeccion_Data();
        public lc_pro_incidente_Data lc_pro_incidente_Data = new lc_pro_incidente_Data();

        public MasterDetailPage1(string s_cod, object v_entidad = null)
        {
            InitializeComponent();
            OnBackButtonPressed();
            ruta = s_cod;
            NavigationPage.SetHasBackButton(this, false);

            if (s_cod != null)
            {
                CargarMaster(s_cod, v_entidad);
            }
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        protected override bool OnBackButtonPressed()
        {
            // By returning TRUE and not calling base we cancel the hardware back button :)
            // base.OnBackButtonPressed();
            return true;
        }

        private void CargarMaster(string s_cod, object v_entidad)
        {
            var Titulo = "";
            Type TargetType = typeof(MasterDetailPage1Detail);
            lc_acc_menu entidad = new lc_acc_menu();
            switch (s_cod)
            {
                case "pg_sincronizar":
                    TargetType = typeof(pg_sincronizar);
                    Titulo = "Sincronizar";
                    break;
                case "pg_empresa":
                    TargetType = typeof(pg_empresa);
                    Titulo = "Seleccionar Unidad";
                    break;
                case "pg_pro_tarea_ate":
                    TargetType = typeof(pg_pro_tarea_ate);
                    Titulo = "Atender Tarea";
                    VarGlobal.comentar = 0;
                    break;
                case "pg_pro_tarea_ver":
                    TargetType = typeof(pg_pro_tarea_ate);
                    Titulo = "Verificar Tarea";
                    VarGlobal.comentar = 0;
                    break;
                case "pg_pro_tarea_com":
                    TargetType = typeof(pg_pro_tarea_ate);
                    Titulo = "Comentar Tarea";
                    VarGlobal.comentar = 1;
                    break;
                case "pg_pro_tarea_mas":
                    TargetType = typeof(pg_pro_tarea_mas);
                    Titulo = "Detalles de Tarea";
                    break;
                case "pg_pro_hallazgo_qry":
                    TargetType = typeof(pg_pro_hallazgo_qry);
                    if (VarGlobal.pro_hallazgo.cod_modulo == "OB")
                    {
                        Titulo = "Mis Observaciones Prev.";
                    }
                    else
                    {
                        Titulo = "Hallazgos";
                    }
                    break;
                case "pg_pro_incidente_qry":
                    TargetType = typeof(pg_pro_incidente_qry);
                    Titulo = "Mis Reportes de Incidentes";
                    break;
                case "pg_pro_inspeccion_qry":
                    TargetType = typeof(pg_pro_inspeccion_qry);
                    Titulo = "Mis Inspecciones";
                    break;
                case "pg_pro_tarea_opc":
                    TargetType = typeof(pg_pro_tarea_opc);
                    Titulo = "Mis Tareas";
                    break;
                case "pg_pro_tarea_qry":
                    TargetType = typeof(pg_pro_tarea_qry);
                    Titulo = VarGlobal.ret_titulo;
                    break;
                case "pg_pro_tarea_mnt":
                    TargetType = typeof(pg_pro_tarea_mnt);
                    Titulo = VarGlobal.pro_tarea.titulo;
                    break;
                case "pg_pro_proyecto_qry":
                    //TargetType = typeof(pg_pro_proyecto_qry);
                    Titulo = "Mis Proyectos";
                    break;
                case "pg_pro_proyecto_dep":
                    //TargetType = typeof(pg_pro_proyecto_dep);
                    //Titulo = VarProyecto.titulo;
                    break;
                case "pg_pro_veoregistro_qry":
                    TargetType = typeof(pg_pro_veoregistro_qry);
                    Titulo = "Mis Evaluaciones VEO";
                    break;
            }
            var page = (Page)Activator.CreateInstance(TargetType);
            page.Title = Titulo;
            Detail = new IconNavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex("#0199DC"),
            };
            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;

        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            VarGlobal.alerta_registro = true;
            Type TargetType = typeof(MasterDetailPage1Detail); ;
            var item = e.SelectedItem as lc_acc_menu;
            if (item == null)
                return;

            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            if (item.nom_menu == "Cerrar Sesión")
            {
                var popupAlert = new pg_confirmacion(new ent_mensaje
                {
                    tip_mensaje = "INF",
                    tit_mensaje = "@tento24",
                    tex_mensaje = "¿Desea Cerrar Sesión?... se perderan los registros grabados en el dispositivo."
                });
                var result = await popupAlert.Show();
                await Navigation.PushPopupAsync(loadingPage);
                if (result)
                {
                    VarGlobal.Limpiar();
                    DB = LocalDB.Instance;
                    var usuario = DB.lc_acc_usuario.ToList();
                    string s_cod_usuario = "";
                    for (int i = 0; i < usuario.Count; i++)
                    {
                        s_cod_usuario = usuario[i].cod_usuario;
                    }
                    var eliminar = DB.lc_acc_usuario.Delete(x => x.cod_usuario == s_cod_usuario);
                    VarGlobal.EliminarTodosRegistros();

                    await Navigation.PushModalAsync(new MainPage("sin_login"));
                    await Navigation.RemovePopupPageAsync(loadingPage);
                    return;
                }
                else
                {
                    MainPage = new MasterDetailPage1(ruta);
                    await Navigation.RemovePopupPageAsync(loadingPage);
                    return;
                }
            }

            VarGlobal.cod_modulo_2do = "";

            switch (item.cod_menu)
            {
                case "SINCRO":
                    //Proyecto
                    TargetType = typeof(pg_sincronizar);
                    VarGlobal.cod_modulo = "";
                    break;
                case "UNIDAD":
                    //Proyecto
                    //  Direccionar a Seleccionar Empresa
                    //lc_acc_menu_Data o_Data = new lc_acc_menu_Data();
                    //o_Data.Eliminar();       
                    VarGlobal.cod_empresa = "";
                    MainPage = new MasterDetailPage1("pg_empresa");
                    TargetType = typeof(pg_empresa);
                    //VarGlobal.cod_unidad = "";
                    //VarGlobal.cod_modulo = "";
                    break;
                case "15_30_":
                    //Proyecto
                    //TargetType = typeof(pg_pro_proyecto_qry);
                    VarGlobal.cod_modulo = "PG";
                    break;
                case "72_10_":
                    //Inspección
                    TargetType = typeof(pg_pro_inspeccion_qry);
                    VarGlobal.pro_hallazgo = new lc_pro_hallazgo();
                    VarGlobal.cod_modulo = "IP";
                    VarGlobal.pro_hallazgo.cod_modulo = "IP";
                    //VarGlobal.pro_hallazgo.cod_modulo = "IP";
                    VarGlobal.cod_modulo_2do = "IP";
                    break;
                case "66_10_":
                    //Observacion
                    TargetType = typeof(pg_pro_hallazgo_qry);
                    VarGlobal.pro_hallazgo = new lc_pro_hallazgo();
                    VarGlobal.pro_hallazgo.cod_modulo = "OB";
                    VarGlobal.cod_modulo = "OB";
                    VarGlobal.cod_modulo_2do = "OB";
                    VarGlobal.pro_hallazgo.cod_referencia = "";
                    break;
                case "69_10_":
                    //Incidente
                    TargetType = typeof(pg_pro_incidente_qry);
                    VarGlobal.cod_modulo = "IN";
                    //VarGlobal.cod_modulo_2do = "IN";
                    break;
                case "85_10_":
                    //Tarea
                    TargetType = typeof(pg_pro_tarea_opc);
                    break;
                case "62_10_":
                    //VEO
                    TargetType = typeof(pg_pro_veoregistro_qry);
                    VarGlobal.cod_modulo = "VE";
                    break;

            }

            var page = (Page)Activator.CreateInstance(TargetType);
            page.Title = item.nom_menu;

            await Navigation.RemovePopupPageAsync(loadingPage);
            Detail = new IconNavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex("#0199DC")
            };
            ContarRegistros();
            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;

        }

        private void ContarRegistros()
        {
            if (VarGlobal.alerta_registro)
            {
                int n_incidente = lc_pro_incidente_Data.Listar().Where(x => x.sincronizado == false).Count();
                int n_inspeccion = lc_pro_inspeccion_Data.Listar().Where(x => x.sincronizado == false).Count();
                int n_hallazgo = lc_pro_hallazgo_Data.Listar().Where(x => x.sincronizado == false).Count();
                int n_tarea = lc_pro_tarea_Data.Listar().Where(x => x.sincronizado == false).Count();
                int n_veo = lc_pro_veoregistro_Data.Listar().Where(x => x.sincronizado == false).Count();

                int total = n_incidente + n_inspeccion + n_hallazgo + n_tarea + n_veo;
                VarGlobal.alerta_can_registro = total;
                if (total > 0)
                {
                    VarGlobal._registro = new pg_registro();
                    Navigation.PushPopupAsync(VarGlobal._registro);
                }
            }

        }
    }
}
