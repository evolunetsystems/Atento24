using System;
using atento24.Data.LiteConnection;
using atento24.Data.ORM;

namespace atento24.Data.StandarDB
{
    public class LocalDB : DataBase
    {
        //ACCESOS
        public Table<lc_acc_usuario> lc_acc_usuario { get; set; }
        public Table<lc_acc_empresa> lc_acc_empresa { get; set; }
        public Table<lc_acc_unidad> lc_acc_unidad { get; set; }
        public Table<lc_acc_menu> lc_acc_menu { get; set; }
        public Table<lc_acc_unidad_sisgestion> lc_acc_unidad_sisgestion { get; set; }
        public Table<lc_acc_unidad_tipoubicacion> lc_acc_unidad_tipoubicacion { get; set; }//

        //AUXILIARES
        public Table<lc_aux_tbldetalle> lc_aux_tbldetalle { get; set; }
        public Table<lc_aux_estado> lc_aux_estado { get; set; }
        public Table<lc_acc_unidad_severidadreal> lc_aux_severidadreal { get; set; }
        public Table<lc_acc_unidad_severidadpot> lc_aux_severidadpot { get; set; }

        //GLOBALES
        public Table<lc_glb_hallazgoclase> lc_glb_hallazgoclase { get; set; }
        public Table<lc_glb_inspecciontipo> lc_glb_inspecciontipo { get; set; }

        //CATALAGOS
        public Table<lc_cat_equipo> lc_cat_equipo { get; set; }
        public Table<lc_cat_labor> lc_cat_labor { get; set; }
        public Table<lc_cat_lugar> lc_cat_lugar { get; set; }
        public Table<lc_cat_ocurrencia> lc_cat_ocurrencia { get; set; }
        public Table<lc_cat_personal> lc_cat_personal { get; set; }
        public Table<lc_cat_veoplantilla> lc_cat_veoplantilla { get; set; }
        public Table<lc_cat_veoplantilla_lncontrol> lc_cat_veoplantilla_lncontrol { get; set; }
        public Table<lc_cat_inspeccionpre> lc_cat_inspeccionpre { get; set; }

        //PROCESOS 
        public Table<lc_pro_coordenada> lc_pro_coordenada { get; set; }
        public Table<lc_pro_evidencia> lc_pro_evidencia { get; set; }
        public Table<lc_pro_hallazgo> lc_pro_hallazgo { get; set; }
        public Table<lc_pro_participante> lc_pro_participante { get; set; }
        public Table<lc_pro_tarea> lc_pro_tarea { get; set; }
        public Table<lc_pro_veoregistro> lc_pro_veoregistro { get; set; }
        public Table<lc_pro_veoregistro_lncontrol> lc_pro_veoregistro_lncontrol { get; set; }
        public Table<lc_pro_inspeccion> lc_pro_inspeccion { get; set; }
        public Table<lc_pro_avance> lc_pro_avance { get; set; }
        public Table<lc_pro_incidente> lc_pro_incidente { get; set; }
        public Table<lc_pro_incidente_personal> lc_pro_incidente_personal { get; set; }
        public Table<lc_pro_estado> lc_pro_estado { get; set; }
        public Table<lc_pro_elimina> lc_pro_elimina { get; set; }

        public LocalDB(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
        {
            //ACCESOS
            lc_acc_usuario = DBSet<lc_acc_usuario>();
            lc_acc_empresa = DBSet<lc_acc_empresa>();
            lc_acc_unidad = DBSet<lc_acc_unidad>();
            lc_acc_menu = DBSet<lc_acc_menu>();
            lc_acc_unidad_sisgestion = DBSet<lc_acc_unidad_sisgestion>();
            lc_acc_unidad_tipoubicacion = DBSet<lc_acc_unidad_tipoubicacion>();

            //AUXILIARES
            lc_aux_tbldetalle = DBSet<lc_aux_tbldetalle>();
            lc_aux_estado = DBSet<lc_aux_estado>();
            lc_aux_severidadpot = DBSet<lc_acc_unidad_severidadpot>();
            lc_aux_severidadreal = DBSet<lc_acc_unidad_severidadreal>();

            //GLOBALES
            lc_glb_hallazgoclase = DBSet<lc_glb_hallazgoclase>();
            lc_glb_inspecciontipo = DBSet<lc_glb_inspecciontipo>();

            //CATALAGOS
            lc_cat_equipo = DBSet<lc_cat_equipo>();
            lc_cat_labor = DBSet<lc_cat_labor>();
            lc_cat_lugar = DBSet<lc_cat_lugar>();
            lc_cat_ocurrencia = DBSet<lc_cat_ocurrencia>();
            lc_cat_personal = DBSet<lc_cat_personal>();
            lc_cat_veoplantilla = DBSet<lc_cat_veoplantilla>();
            lc_cat_veoplantilla_lncontrol = DBSet<lc_cat_veoplantilla_lncontrol>();
            lc_cat_inspeccionpre = DBSet<lc_cat_inspeccionpre>();

            //PROCESOS 
            lc_pro_coordenada = DBSet<lc_pro_coordenada>();
            lc_pro_evidencia = DBSet<lc_pro_evidencia>();
            lc_pro_hallazgo = DBSet<lc_pro_hallazgo>();
            lc_pro_participante = DBSet<lc_pro_participante>();
            lc_pro_tarea = DBSet<lc_pro_tarea>();
            lc_pro_veoregistro = DBSet<lc_pro_veoregistro>();
            lc_pro_veoregistro_lncontrol = DBSet<lc_pro_veoregistro_lncontrol>();
            lc_pro_inspeccion = DBSet<lc_pro_inspeccion>();
            lc_pro_avance = DBSet<lc_pro_avance>();
            lc_pro_incidente = DBSet<lc_pro_incidente>();
            lc_pro_incidente_personal = DBSet<lc_pro_incidente_personal>();
            lc_pro_estado = DBSet<lc_pro_estado>();
            lc_pro_elimina = DBSet<lc_pro_elimina>();
        }

        public static LocalDB Instance
        {
            get
            {
                var service = Xamarin.Forms.DependencyService.Get<IDataBase>();
                if (service != null)
                {
                    return service.GetDataBase();
                }
                throw new Exception("No es posible obtener el servicio");
            }
        }
    }
}
