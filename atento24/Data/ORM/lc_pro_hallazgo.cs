using System.Collections.Generic;

namespace atento24.Data.ORM
{
    public class lc_pro_hallazgo : lc_tabla_base
    {

        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_hallazgo { get; set; }
        public string fec_hallazgo { get; set; }
        public string cod_hallazgoclase { get; set; }
        public string nom_hallazgoclase { get; set; }
        public string cod_tblnivelriesgo { get; set; }
        public string nom_tblnivelriesgo { get; set; }
        public string cod_periodo { get; set; }
        public string cod_estado { get; set; }
        public string nom_estado { get; set; }
        public string cod_ocurrencia { get; set; }
        public string nom_ocurrencia { get; set; }
        public string cod_tblocurrenciatipo { get; set; }
        public string nom_tblocurrenciatipo { get; set; }
        public string des_hallazgo { get; set; }
        public string cod_sisgestion { get; set; }
        public string nom_sisgestion { get; set; }
        public string cod_tipoubicacion { get; set; }
        public string nom_tipoubicacion { get; set; }
        public string cod_labor { get; set; }
        public string nom_labor { get; set; }
        public string cod_lugar { get; set; }
        public string nom_lugar { get; set; }
        public string cod_equipo { get; set; }
        public string nom_equipo { get; set; }
        public string nom_ubicacion { get; set; }
        public string cod_personal { get; set; }
        public string nom_personal { get; set; }
        public string cod_modulo { get; set; }
        public string nom_modulo { get; set; }
        public string cod_referencia { get; set; }
        public string des_origen { get; set; }
        public int por_avance { get; set; }
        public bool sincronizado { get; set; }
        public bool ver_clase { get; set; }

        public List<lc_pro_evidencia> lst_lc_pro_evidencia = new List<lc_pro_evidencia>();
        public List<lc_pro_tarea> lst_lc_pro_tarea = new List<lc_pro_tarea>();
        public List<lc_pro_participante> lst_lc_pro_participante = new List<lc_pro_participante>();
        public List<lc_pro_coordenada> lst_lc_pro_coordenada = new List<lc_pro_coordenada>();

        public string niv_color { get; set; }
        public string sincr_color { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }

        public bool ver_tarea { get; set; }
        public int num_tarea { get; set; }

        public string titulo { get; set; }
        public string cod_usuario { get; set; }
        public string hora { get; set; }
        public string eje_personal { get; set; }
        public string nom_eje_personal { get; set; }
        public string des_tarea { get; set; }

    }
}
