using System;
using System.Collections.Generic;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Pages.Procesos;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Busquedas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_personal : ContentPage
    {
        public static string modulo;
        public static string tipo_persona;

        public List<lc_cat_personal> lista_cat_personal;

        public pg_personal(string cod_modulo, string tipo)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            switch (tipo)
            {
                case "Rep":
                case "Ejec":
                    lb_titulo.Text = "Responsable";
                    break;
                case "Inf":
                    lb_titulo.Text = "Infractor";
                    break;
                case "Eva":
                case "Eval":
                    lb_titulo.Text = "Evaluador";
                    break;
                case "Eje":
                    lb_titulo.Text = "Ejecutor";
                    break;
                case "Par":
                    lb_titulo.Text = "Participante";
                    break;
                case "Part":
                    lb_titulo.Text = "Personal";
                    break;
            }

            CargarPersonal();
            modulo = cod_modulo;
            tipo_persona = tipo;
        }

        private void CargarPersonal()
        {
            lc_cat_personal_Data o_Data = new lc_cat_personal_Data();
            lista_cat_personal = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa).ToList();
            PersonalListView.ItemsSource = lista_cat_personal;
        }

        private void btnFiltro_Clicked(object sender, EventArgs e)
        {
            var lista = lista_cat_personal.Where(x => x.nom_personal.ToUpper().Contains(edFiltro.Text.Trim().ToUpper())).ToList();
            PersonalListView.ItemsSource = lista;
        }

        private void PersonalListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lc_cat_personal entidad = ((ListView)sender).SelectedItem as lc_cat_personal;
            List<lc_cat_personal> lista = new List<lc_cat_personal>();
            lista.Add(entidad);
            bool b_error = false;

            switch (tipo_persona)
            {
                case "Rep":
                    switch (modulo)
                    {
                        case "OB":
                            VarGlobal.pro_hallazgo.cod_personal = entidad.cod_personal;
                            VarGlobal.pro_hallazgo.nom_personal = entidad.nom_personal;
                            Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                            break;
                        case "IP":
                            VarGlobal.pro_inspeccion.cod_personal = entidad.cod_personal;
                            VarGlobal.pro_inspeccion.nom_personal = entidad.nom_personal;
                            Navigation.PushAsync(new pg_pro_inspeccion_mnt("B") { Title = VarGlobal.pro_inspeccion.titulo });
                            break;
                        case "PG":
                            //VarProyecto.cod_personal = entidad.cod_personal;
                            //VarProyecto.nom_personal = entidad.nom_personal;
                            //Navigation.PushAsync(new pg_pro_proyecto_mnt("B") { Title = VarProyecto.ret_titulo });
                            break;
                        case "IN":
                            VarGlobal.pro_incidente.cod_personal = entidad.cod_personal;
                            VarGlobal.pro_incidente.nom_personal = entidad.nom_personal;
                            Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.ret_titulo });
                            break;
                    }
                    break;
                case "Inf":

                    //  Verificar si Existe en la Lista de Infractores
                    for (int i = 0; i < VarGlobal.pro_hallazgo.lst_lc_pro_participante.Count; i++)
                    {
                        if (VarGlobal.pro_hallazgo.lst_lc_pro_participante[i].cod_personal == entidad.cod_personal)
                        {
                            b_error = true;
                            VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                            {
                                tip_mensaje = "ERR",
                                tit_mensaje = "Error de validación",
                                tex_mensaje = "Infractor: " + VarGlobal.pro_hallazgo.lst_lc_pro_participante[i].nom_personal + " ya fue seleccionado...",
                            });
                            Navigation.PushPopupAsync(VarGlobal._mensaje);
                            //DisplayAlert("Error", "Infractor ya fue seleccionado", "Aceptar");
                            return;
                        }
                    }
                    //  Agregamos a lista de Infractores
                    VarGlobal.pro_hallazgo.lst_lc_pro_participante.Add(new lc_pro_participante()
                    {
                        cod_empresa = VarGlobal.cod_empresa,
                        cod_unidad = VarGlobal.cod_unidad,
                        cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo,
                        tip_participante = "I",
                        cod_personal = entidad.cod_personal,
                        nom_personal = entidad.nom_personal,
                        des_participante = "Generado desde el App Hallazgo",
                        cod_modulo = modulo,
                        sincronizado = false,
                        usuario = VarGlobal.cod_usuario,
                        ip = VarGlobal.ip,
                        estado = "A",
                        comando = "INS"
                    });
                    Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
                case "Eva":
                    //  Verificar si Existe en la Lista de Participantes
                    for (int i = 0; i < VarGlobal.pro_inspeccion.lst_lc_pro_participante.Count; i++)
                    {
                        if (VarGlobal.pro_inspeccion.lst_lc_pro_participante[i].cod_personal == entidad.cod_personal)
                        {
                            b_error = true;
                            VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                            {
                                tip_mensaje = "ERR",
                                tit_mensaje = "Error de validación",
                                tex_mensaje = "Participante: " + VarGlobal.pro_inspeccion.lst_lc_pro_participante[i].nom_personal + " ya fue seleccionado...",
                            });
                            Navigation.PushPopupAsync(VarGlobal._mensaje);
                            //DisplayAlert("Error", "Participante ya fue seleccionado", "Aceptar");
                            return;
                        }
                    }

                    //Agregamos a lista de Participantes
                    VarGlobal.pro_inspeccion.lst_lc_pro_participante.Add(new lc_pro_participante()
                    {
                        cod_empresa = VarGlobal.cod_empresa,
                        cod_unidad = VarGlobal.cod_unidad,
                        cod_referencia = VarGlobal.pro_inspeccion.cod_inspeccion,
                        tip_participante = "E",
                        cod_personal = entidad.cod_personal,
                        nom_personal = entidad.nom_personal,
                        des_participante = "Generado desde el App Inspección",
                        cod_modulo = modulo,
                        sincronizado = false,
                        usuario = VarGlobal.cod_usuario,
                        ip = VarGlobal.ip,
                        estado = "A",
                        comando = "INS"
                    });
                    Navigation.PushAsync(new pg_pro_inspeccion_mnt("B") { Title = VarGlobal.pro_inspeccion.titulo });
                    break;
                case "Eje":
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].cod_personal == VarGlobal.cod_personal)
                        {
                            b_error = true;
                            VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                            {
                                tip_mensaje = "ERR",
                                tit_mensaje = "Error de validación",
                                tex_mensaje = "Solicitante: " + lista[i].nom_personal + " ya fue seleccionado...",
                            });
                            Navigation.PushPopupAsync(VarGlobal._mensaje);
                            //DisplayAlert("Error", "Solicitante no puede Ejecutor ", "Aceptar");
                            return;
                        }
                        VarGlobal.pro_tarea.eje_personal = lista[i].cod_personal;
                        VarGlobal.pro_tarea.nom_eje_personal = lista[i].nom_personal;
                    }
                    Navigation.PushAsync(new pg_pro_tarea_mnt("B") { Title = VarGlobal.ret_titulo });
                    break;                
                case "Eval":
                    //  Verificar si Existe en la Lista de Participantes
                    for (int i = 0; i < VarGlobal.pro_veoregistro.lst_lc_pro_participante.Count; i++)
                    {
                        if (VarGlobal.pro_veoregistro.lst_lc_pro_participante[i].cod_personal == entidad.cod_personal)
                        {
                            b_error = true;
                            VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                            {
                                tip_mensaje = "ERR",
                                tit_mensaje = "Error de validación",
                                tex_mensaje = "Participante: " + VarGlobal.pro_veoregistro.lst_lc_pro_participante[i].nom_personal + " ya fue seleccionado...",
                            });
                            Navigation.PushPopupAsync(VarGlobal._mensaje);
                            //DisplayAlert("Error", "Participante ya fue seleccionado", "Aceptar");
                            return;
                        }
                    }
                    //  Agregamos a lista de Participantes
                    VarGlobal.pro_veoregistro.lst_lc_pro_participante.Add(new lc_pro_participante()
                    {
                        cod_empresa = VarGlobal.cod_empresa,
                        cod_unidad = VarGlobal.cod_unidad,
                        cod_referencia = VarGlobal.pro_veoregistro.cod_veoregistro,
                        tip_participante = "E",
                        cod_personal = entidad.cod_personal,
                        nom_personal = entidad.nom_personal,
                        des_participante = "Generado desde el App Inspección",
                        cod_modulo = modulo,
                        sincronizado = false,
                        usuario = VarGlobal.cod_usuario,
                        ip = VarGlobal.ip,
                        estado = "A",
                        comando = "INS"
                    });
                    if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("N") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    else
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("B") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    break;
                case "Part":
                    //  Verificar si Existe en la Lista de Personal
                    if (VarGlobal.pro_incidente.lst_lc_pro_incidente_personal != null)
                    {
                        for (int i = 0; i < VarGlobal.pro_incidente.lst_lc_pro_incidente_personal.Count; i++)
                        {
                            if (VarGlobal.pro_incidente.lst_lc_pro_incidente_personal[i].cod_personal == entidad.cod_personal)
                            {
                                b_error = true;
                                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                                {
                                    tip_mensaje = "ERR",
                                    tit_mensaje = "Error de validación",
                                    tex_mensaje = "Personal: " + VarGlobal.pro_incidente.lst_lc_pro_incidente_personal[i].nom_personal + " ya fue seleccionado...",
                                });
                                Navigation.PushPopupAsync(VarGlobal._mensaje);
                                //DisplayAlert("Error", "Personal ya fue seleccionado", "Aceptar");
                                return;
                            }
                        }
                    }

                    //  Agregamos a lista de Participantes
                    VarGlobal.pro_incidente.lst_lc_pro_incidente_personal.Add(new lc_pro_incidente_personal()
                    {
                        cod_empresa = VarGlobal.cod_empresa,
                        cod_unidad = VarGlobal.cod_unidad,
                        cod_incidente = VarGlobal.pro_incidente.cod_incidente,
                        cod_personal = entidad.cod_personal,
                        nom_personal = entidad.nom_personal,
                        dgn_personal = "Generado desde el App",
                        tra_personal = "Generado desde el App",
                        dpe_personal = 0,
                        sincronizado = false,
                        usuario = VarGlobal.cod_usuario,
                        ip = VarGlobal.ip,
                        estado = "A",
                        comando = "INS"
                    });
                    Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
                    break;
                case "Ejec":
                    VarGlobal.pro_hallazgo.eje_personal = entidad.cod_personal;
                    VarGlobal.pro_hallazgo.nom_eje_personal = entidad.nom_personal;
                    Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
            }
        }

        private void btnsalir_Clicked(object sender, EventArgs e)
        {
            switch (modulo)
            {
                case "OB":
                    Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
                case "IP":
                    Navigation.PushAsync(new pg_pro_inspeccion_mnt("B") { Title = VarGlobal.pro_inspeccion.titulo });
                    break;
                case "HL":
                case "TR":
                    Navigation.PushAsync(new pg_pro_tarea_mnt("B") { Title = VarGlobal.ret_titulo });
                    break;
                case "PG":
                    //Navigation.PushAsync(new pg_pro_proyecto_mnt("B") { Title = VarProyecto.ret_titulo });
                    break;
                case "VE":
                    if (VarGlobal.pro_veoregistro.cod_veoregistro == "")
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("N") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    else
                    {
                        Navigation.PushAsync(new pg_pro_veoregistro_mnt("B") { Title = VarGlobal.pro_veoregistro.titulo });
                    }
                    break;
                case "IN":
                    Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
                    break;
            }

        }

        private void PersonalListView_Refreshing(object sender, EventArgs e)
        {
            CargarPersonal();
            PersonalListView.IsRefreshing = false;
        }
    }
}
