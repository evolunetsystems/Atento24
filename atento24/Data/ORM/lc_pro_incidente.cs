using System.Collections.Generic;

namespace atento24.Data.ORM
{
    public class lc_pro_incidente : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_incidente { get; set; }
        public string fec_incidente { get; set; }
        public string cod_sisgestion { get; set; }
        public string cod_severidadreal { get; set; }
        public string cod_severidadpot { get; set; }
        public string des_incidente { get; set; }
        public string cod_tipoubicacion { get; set; }
        public string cod_labor { get; set; }
        public string cod_lugar { get; set; }
        public string cod_equipo { get; set; }
        public string cod_personal { get; set; }
        public string cod_periodo { get; set; }
        public string cod_estado { get; set; }
        public int por_avance { get; set; }

        public string nom_periodo { get; set; }
        public string nom_severidadreal { get; set; }
        public string nom_severidadpot { get; set; }
        public string nom_sisgestion { get; set; }
        public string nom_tipoubicacion { get; set; }
        public string nom_ubicacion { get; set; }
        public string nom_labor { get; set; }
        public string nom_lugar { get; set; }
        public string nom_equipo { get; set; }
        public string nom_personal { get; set; }
        public string nom_estado { get; set; }
        public string nom_modulo { get; set; }
        public string nom_referencia { get; set; }
        public bool sincronizado { get; set; }
        public string sincr_color { get; set; }

        public string titulo { get; set; }
        public string ret_titulo { get; set; }
        public string hora { get; set; }
        public string retorno { get; set; }
        public List<lc_pro_evidencia> lst_lc_pro_evidencia = new List<lc_pro_evidencia>();
        public List<lc_pro_tarea> lst_lc_pro_tarea = new List<lc_pro_tarea>();
        public List<lc_pro_incidente_personal> lst_lc_pro_incidente_personal = new List<lc_pro_incidente_personal>();

        public bool ver_tarea { get; set; }
        public int num_tarea { get; set; }
    }
}
