using System.Collections.Generic;

namespace atento24.Data.ORM
{
    public class lc_pro_veoregistro : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public string cod_veoregistro { get; set; }
        public string fec_veoregistro { get; set; }
        public string cod_periodo { get; set; }
        public string cod_veoplantilla { get; set; }
        public string des_veoplantilla { get; set; }
        public string cod_tipoubicacion { get; set; }
        public string cod_sisgestion { get; set; }
        public string cod_labor { get; set; }
        public string cod_lugar { get; set; }
        public string cod_equipo { get; set; }
        public int cum_veoregistro { get; set; }
        public int noc_veoregistro { get; set; }
        public int noa_veoregistro { get; set; }
        public decimal por_veoregistro { get; set; }
        public string cer_veoregistro { get; set; }
        public int par_veoregistro { get; set; }

        public string nom_periodo { get; set; }
        public string nom_veoplantilla { get; set; }
        public string nom_sisgestion { get; set; }
        public string nom_tipoubicacion { get; set; }
        public string nom_ubicacion { get; set; }
        public string nom_labor { get; set; }
        public string nom_lugar { get; set; }
        public string nom_equipo { get; set; }

        public string cod_evaluador { get; set; }
        public string nom_evaluador { get; set; }
        public List<lc_pro_participante> lst_lc_pro_participante = new List<lc_pro_participante>();
        public List<lc_pro_veoregistro_lncontrol> lst_lc_pro_veoregistro_lncontrol = new List<lc_pro_veoregistro_lncontrol>();
        public List<lc_pro_coordenada> lst_lc_pro_coordenada = new List<lc_pro_coordenada>();

        public string cod_usuario { get; set; }
        public string titulo { get; set; }
        public string retorno { get; set; }
        public string hora { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }
        public bool sincronizado { get; set; }
        public string sincr_color { get; set; }
    }
}
