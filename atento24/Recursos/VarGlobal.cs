
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;

namespace atento24.Recursos
{
    public static class VarGlobal
    {
        #region DeclarandoData
        private static lc_acc_empresa_Data lc_acc_empresa_Data = new lc_acc_empresa_Data();
        private static lc_acc_unidad_Data lc_acc_unidad_Data = new lc_acc_unidad_Data();
        private static lc_acc_menu_Data lc_acc_menu_Data = new lc_acc_menu_Data();
        private static lc_acc_usuario_Data lc_acc_usuario_Data = new lc_acc_usuario_Data();
        private static lc_cat_equipo_Data lc_cat_equipo_Data = new lc_cat_equipo_Data();
        private static lc_cat_labor_Data lc_cat_labor_Data = new lc_cat_labor_Data();
        private static lc_cat_lugar_Data lc_cat_lugar_Data = new lc_cat_lugar_Data();
        private static lc_cat_ocurrencia_Data lc_cat_ocurrencia_Data = new lc_cat_ocurrencia_Data();
        private static lc_cat_personal_Data lc_cat_personal_Data = new lc_cat_personal_Data();
        private static lc_cat_veoplantilla_Data lc_cat_veoplantilla_Data = new lc_cat_veoplantilla_Data();
        private static lc_cat_veoplantilla_lncontrol_Data lc_cat_veoplantilla_lncontrol_Data = new lc_cat_veoplantilla_lncontrol_Data();

        private static lc_acc_unidad_tipoubicacion_Data lc_acc_unidad_tipoubicacion_Data = new lc_acc_unidad_tipoubicacion_Data();
        private static lc_acc_unidad_sisgestion_Data lc_acc_unidad_sisgestion_Data = new lc_acc_unidad_sisgestion_Data();
        private static lc_aux_tbldetalle_Data lc_aux_tbldetalle_Data = new lc_aux_tbldetalle_Data();
        private static lc_glb_hallazgoclase_Data lc_glb_hallazgoclase_Data = new lc_glb_hallazgoclase_Data();
        private static lc_aux_estado_Data lc_aux_estado_Data = new lc_aux_estado_Data();
        private static lc_acc_unidad_severidadpot_Data lc_acc_unidad_severidadpot_Data = new lc_acc_unidad_severidadpot_Data();
        private static lc_acc_unidad_severidadreal_Data lc_acc_unidad_severidadreal_Data = new lc_acc_unidad_severidadreal_Data();

        private static lc_pro_hallazgo_Data lc_pro_hallazgo_Data = new lc_pro_hallazgo_Data();
        private static lc_pro_coordenada_Data lc_pro_coordenada_Data = new lc_pro_coordenada_Data();
        private static lc_pro_tarea_Data lc_pro_tarea_Data = new lc_pro_tarea_Data();
        private static lc_pro_evidencia_Data lc_pro_evidencia_Data = new lc_pro_evidencia_Data();
        private static lc_pro_participante_Data lc_pro_participante_Data = new lc_pro_participante_Data();
        private static lc_pro_veoregistro_Data lc_pro_veoregistro_Data = new lc_pro_veoregistro_Data();
        private static lc_pro_veoregistro_lncontrol_Data lc_pro_veoregistro_lncontrol_Data = new lc_pro_veoregistro_lncontrol_Data();
        private static lc_pro_inspeccion_Data lc_pro_inspeccion_Data = new lc_pro_inspeccion_Data();
        private static lc_pro_avance_Data lc_pro_avance_Data = new lc_pro_avance_Data();
        private static lc_pro_incidente_Data lc_pro_incidente_Data = new lc_pro_incidente_Data();
        private static lc_pro_incidente_personal_Data lc_pro_incidente_personal_Data = new lc_pro_incidente_personal_Data();
        private static lc_pro_estado_Data lc_pro_estado_Data = new lc_pro_estado_Data();
        private static lc_pro_elimina_Data lc_pro_elimina_Data = new lc_pro_elimina_Data();
        #endregion

        public static string cod_usuario { get; set; }
        public static string nom_usuario { get; set; }
        public static string cod_empresa { get; set; }
        public static string nom_empresa { get; set; }
        public static string cod_unidad { get; set; }
        public static string nom_unidad { get; set; }
        public static string cod_perfil { get; set; }
        public static string nom_perfil { get; set; }
        public static string cod_personal { get; set; }
        public static string nom_personal { get; set; }

        public static string cod_tarea { get; set; }
        public static string des_tarea { get; set; }
        public static decimal por_avance { get; set; }
        public static string cod_estado { get; set; }
        public static int eta_estado { get; set; }
        public static int comentar { get; set; }
        public static bool MostrarMenu { get; set; }
        public static string cod_tbldetalle { get; set; }

        public static byte[] dat_evidencia { get; set; }
        public static string nom_evidencia { get; set; }
        public static string tip_evidencia { get; set; }
        public static decimal tam_evidencia { get; set; }
        public static string com_evidencia { get; set; }
        public static string tip_etapa { get; set; }
        public static int num_etapa { get; set; }
        public static string retorno { get; set; }
        public static string cod_modulo { get; set; }
        public static string cod_modulo_2do { get; set; }
        public static string ret_titulo { get; set; }

        /// <summary>
        /// Variable de retorno de tarea dependiente de un origen
        /// </summary>
        public static string ret_tarea_padre { get; set; }
        /// <summary>
        /// Variable de retorno de tarea
        /// </summary>
        public static string ret_tarea_hijo { get; set; }

        /// <summary>
        /// Variable de retorno de hallazgo dependiente de un origen
        /// </summary>
        public static string ret_hallazgo_padre { get; set; }
        /// <summary>
        /// Variable de retorno de hallazgo
        /// </summary>
        public static string ret_hallazgo_hijo { get; set; }
        public static string tit_inspeccion_hall { get; set; }
        public static string tip_inspeccion_hall { get; set; }

        /// <summary>
        /// Variable de Retorno del Módulo.
        /// Se usa en en las lista de busqueda como Lugaes, Labores, Equipos, Evidencias, Personal, ETC
        /// </summary>
        public static string cod_modulo_ret { get; set; }

        public static string cod_referencia { get; set; }
        public static string ver_opcion { get; set; }
        public static string ip { get; set; }
        //public static int num_orden { get; set; }
        public static int timeout = 600000;

        public static lc_pro_hallazgo pro_hallazgo;

        /// <summary>
        /// Entidad Publica de registro de tarea
        /// </summary>
        public static lc_pro_tarea pro_tarea;
        public static lc_pro_veoregistro pro_veoregistro;
        public static lc_pro_inspeccion pro_inspeccion;
        public static lc_pro_incidente pro_incidente;

        public static void Limpiar()
        {
            cod_empresa = "";
            nom_empresa = "";
            cod_unidad = "";
            nom_unidad = "";
            cod_usuario = "";
            nom_usuario = "";
            cod_perfil = "";
            nom_perfil = "";
            cod_personal = "";
            nom_personal = "";
            //cod_tarea = "";
            //des_tarea = "";
            por_avance = 0;
            cod_estado = "";
            eta_estado = 0;
            comentar = 0;
        }

        public static HttpResponseMessage LoadWebApiPost(string sApi, object oEntidad)
        {
            HttpClient client_Post = new HttpClient();
            client_Post.Timeout = TimeSpan.FromMinutes(10);
            client_Post.BaseAddress = new Uri("http://webapi.atento24.com/");
            client_Post.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            StringContent content = new StringContent(JsonConvert.SerializeObject(oEntidad), Encoding.UTF8, "application/json");
            return client_Post.PostAsync("api/" + sApi, content).Result;
        }

        public static HttpResponseMessage LoadWebApiPut(string sApi, object oEntidad)
        {
            HttpClient client_Put = new HttpClient();
            client_Put.Timeout = TimeSpan.FromMinutes(10);
            client_Put.BaseAddress = new Uri("http://webapi.atento24.com/");
            client_Put.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            StringContent content = new StringContent(JsonConvert.SerializeObject(oEntidad), Encoding.UTF8, "application/json");
            return client_Put.PutAsync("api/" + sApi, content).Result;
        }

        public static pg_registro _registro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static int alerta_can_registro { get; set; }
        public static bool alerta_registro { get; set; }
        public static pg_mensaje _mensaje { get; set; }
        public static string tit_mensaje { get; set; }

        public static void EliminarTodosRegistros()
        {
            lc_cat_veoplantilla_Data.Eliminar();
            lc_cat_veoplantilla_lncontrol_Data.Eliminar();
            lc_cat_personal_Data.Eliminar();
            lc_cat_ocurrencia_Data.Eliminar();
            lc_cat_labor_Data.Eliminar();
            lc_cat_lugar_Data.Eliminar();
            lc_cat_equipo_Data.Eliminar();
            lc_acc_menu_Data.Eliminar();
            lc_acc_unidad_Data.Eliminar();
            lc_acc_empresa_Data.Eliminar();
            lc_acc_unidad_tipoubicacion_Data.Eliminar();
            lc_acc_unidad_sisgestion_Data.Eliminar();
            lc_aux_tbldetalle_Data.Eliminar();
            lc_aux_estado_Data.Eliminar();
            lc_glb_hallazgoclase_Data.Eliminar();
            lc_pro_hallazgo_Data.Eliminar();
            lc_pro_tarea_Data.Eliminar();
            lc_pro_evidencia_Data.Eliminar();
            lc_pro_coordenada_Data.Eliminar();
            lc_pro_participante_Data.Eliminar();
            lc_pro_veoregistro_Data.Eliminar();
            //lc_pro_veoregistro_lncontrol_Data.Eliminar();
            lc_pro_inspeccion_Data.Eliminar();
            lc_pro_avance_Data.Eliminar();
            lc_pro_incidente_Data.Eliminar();
            //lc_pro_incidente_personal_Data.Eliminar();
            lc_acc_unidad_severidadpot_Data.Eliminar();
            lc_acc_unidad_severidadreal_Data.Eliminar();
            lc_pro_estado_Data.Eliminar();
            lc_pro_elimina_Data.Eliminar();
        }
    }
}