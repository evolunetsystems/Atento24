using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Recursos;
using atento24.Pages.Procesos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Busquedas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_plantillaveo : ContentPage
    {
        public List<lc_cat_veoplantilla> lista_cat_veoplantilla;
        public pg_plantillaveo()
        {
            InitializeComponent();
            VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol = new List<lc_pro_veoregistro_lncontrol>();
            VarGlobal.pro_veoregistro.lst_lc_pro_participante = new List<lc_pro_participante>();
            NavigationPage.SetHasBackButton(this, false);
            switch (VarGlobal.cod_modulo)
            {
                case "VE":
                    lb_titulo.Text = "Plantilla Veo";
                    break;
            }
            CargarPlantillaVeo();
        }

        private void CargarPlantillaVeo()
        {
            lc_cat_veoplantilla_Data o_Data = new lc_cat_veoplantilla_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                && x.cod_unidad == VarGlobal.cod_unidad).ToList();
            VeoPlantillaListView.ItemsSource = lista.OrderByDescending(x => x.cod_veoplantilla).ToList();
        }

        private async void btnsalir_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "WAR",
                tit_mensaje = "Plantillas V.E.O.",
                tex_mensaje = "¿Seguro que deseas salir?"
            });
            //var popupAlert = new pg_confirmacion("Plantillas V.E.O.", "¿Seguro que deseas salir?");
            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                await Navigation.PushModalAsync(new MasterDetailPage1("pg_pro_veoregistro_qry"));
            }
            await Navigation.RemovePopupPageAsync(loadingPage);
        }

        private async void VeoPlantillaListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            lc_cat_veoplantilla entidad = ((ListView)sender).SelectedItem as lc_cat_veoplantilla;
            AsignarVeoPlantilla(entidad);
            UbicacionVeoPlantilla(entidad);
            CargarLineaControl(entidad);
            AsignarParticipante();
            await Navigation.PushAsync(new pg_pro_veoregistro_mnt("N") { Title = VarGlobal.pro_veoregistro.titulo });

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private void AsignarParticipante()
        {
            VarGlobal.pro_veoregistro.lst_lc_pro_participante.Add(new lc_pro_participante()
            {
                cod_empresa = VarGlobal.cod_empresa,
                cod_unidad = VarGlobal.cod_unidad,
                cod_referencia = VarGlobal.pro_veoregistro.cod_veoregistro,
                tip_participante = "E",
                cod_personal = VarGlobal.cod_personal,
                nom_personal = VarGlobal.nom_personal,
                des_participante = "Generado desde el App Inspección",
                cod_modulo = "VE",
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip,
                estado = "A",
            });
        }

        private void CargarLineaControl(lc_cat_veoplantilla entidad)
        {
            lc_cat_veoplantilla_lncontrol_Data o_Data = new lc_cat_veoplantilla_lncontrol_Data();
            var lista = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                && x.cod_unidad == VarGlobal.cod_unidad
                                                && x.cod_veoplantilla == entidad.cod_veoplantilla).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                lc_pro_veoregistro_lncontrol entidad_veo = new lc_pro_veoregistro_lncontrol();

                entidad_veo.cod_empresa = lista[i].cod_empresa;
                entidad_veo.cod_unidad = lista[i].cod_unidad;
                entidad_veo.cod_lncontrol = lista[i].cod_lncontrol;
                entidad_veo.cod_riesgo = lista[i].cod_riesgo;
                entidad_veo.nom_lncontrol = lista[i].nom_lncontrol;
                entidad_veo.ord_lncontrol = lista[i].ord_lncontrol;
                entidad_veo.pes_lncontrol = lista[i].pes_lncontrol;
                entidad_veo.cod_simbolo = lista[i].cod_simbolo;
                entidad_veo.val_simbolo = lista[i].val_simbolo;
                entidad_veo.cod_medida = lista[i].cod_medida;
                entidad_veo.pa1_lncontrol = lista[i].pa1_lncontrol;
                entidad_veo.pa2_lncontrol = lista[i].pa2_lncontrol;
                entidad_veo.cod_tipodato = lista[i].cod_tipodato;
                entidad_veo.nom_simbolo = lista[i].nom_simbolo;
                entidad_veo.val_lncontrol = lista[i].val_lncontrol;
                entidad_veo.ale_lncontrol = lista[i].ale_lncontrol;
                entidad_veo.usuario = VarGlobal.cod_usuario;
                entidad_veo.ip = VarGlobal.ip;
                VarGlobal.pro_veoregistro.lst_lc_pro_veoregistro_lncontrol.Add(entidad_veo);
            }
        }

        private void UbicacionVeoPlantilla(lc_cat_veoplantilla entidad)
        {
            switch (entidad.cod_tipoubicacion)
            {
                case "E":
                    VarGlobal.pro_veoregistro.nom_tipoubicacion = entidad.nom_tipoubicacion;
                    VarGlobal.pro_veoregistro.cod_labor = "";
                    VarGlobal.pro_veoregistro.nom_labor = "";
                    VarGlobal.pro_veoregistro.cod_lugar = "";
                    VarGlobal.pro_veoregistro.nom_lugar = "";
                    break;
                case "I":
                    VarGlobal.pro_veoregistro.nom_tipoubicacion = entidad.nom_tipoubicacion;
                    VarGlobal.pro_veoregistro.cod_lugar = "";
                    VarGlobal.pro_veoregistro.nom_lugar = "";
                    VarGlobal.pro_veoregistro.cod_equipo = "";
                    VarGlobal.pro_veoregistro.nom_equipo = "";
                    break;
                case "S":
                    VarGlobal.pro_veoregistro.nom_tipoubicacion = entidad.nom_tipoubicacion;
                    VarGlobal.pro_veoregistro.cod_labor = "";
                    VarGlobal.pro_veoregistro.nom_labor = "";
                    VarGlobal.pro_veoregistro.cod_equipo = "";
                    VarGlobal.pro_veoregistro.nom_equipo = "";
                    break;
            }
        }

        private void AsignarVeoPlantilla(lc_cat_veoplantilla entidad)
        {
            VarGlobal.pro_veoregistro.cod_veoplantilla = entidad.cod_veoplantilla;
            VarGlobal.pro_veoregistro.nom_veoplantilla = entidad.nom_veoplantilla;
            VarGlobal.pro_veoregistro.des_veoplantilla = entidad.des_veoplantilla;
            VarGlobal.pro_veoregistro.cod_tipoubicacion = entidad.cod_tipoubicacion;
            VarGlobal.pro_veoregistro.nom_tipoubicacion = entidad.nom_tipoubicacion;
            VarGlobal.pro_veoregistro.cod_sisgestion = entidad.cod_sisgestion;
            VarGlobal.pro_veoregistro.nom_sisgestion = entidad.nom_sisgestion;
            VarGlobal.pro_veoregistro.par_veoregistro = entidad.par_veoplantilla;
            VarGlobal.pro_veoregistro.ip = VarGlobal.ip;
            VarGlobal.pro_veoregistro.usuario = VarGlobal.cod_usuario;
        }

        private void btnVeoPla_Clicked(object sender, EventArgs e)
        {
            var lista = lista_cat_veoplantilla.Where(x => x.nom_veoplantilla.ToUpper().Contains(edVeoPla.Text.Trim().ToUpper())).ToList();
            VeoPlantillaListView.ItemsSource = lista;
        }
    }
}
