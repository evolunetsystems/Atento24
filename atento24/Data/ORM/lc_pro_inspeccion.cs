using System.Collections.Generic;

namespace atento24.Data.ORM
{
    public class lc_pro_inspeccion : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public string cod_inspeccion { get; set; }
        public string fec_inspeccion { get; set; }
        public string cod_periodo { get; set; } 

        public string cod_inspecciontipo { get; set; }
        public string cod_inspeccionpre { get; set; }
        public string tit_inspeccion { get; set; }
        public string obj_inspeccion { get; set; }
        public string cod_personal { get; set; }
        public string cod_sisgestion { get; set; }
        public string cod_inspeccionprog { get; set; }

        public string cod_estado { get; set; } 

        public string nom_periodo { get; set; }
        public string nom_inspeccionpre { get; set; }
        public string nom_inspecciontipo { get; set; }
        public string nom_sisgestion { get; set; }
        public string nom_personal { get; set; }
        public string nom_estado { get; set; }

        public List<lc_pro_participante> lst_lc_pro_participante = new List<lc_pro_participante>();
        public List<lc_pro_hallazgo> lst_lc_pro_hallazgo = new List<lc_pro_hallazgo>();
        public string titulo { get; set; }
        public string cod_usuario { get; set; }
        public bool sincronizado { get; set; }
        public string sincr_color { get; set; }
        public int por_avance { get; set; }

        public bool ver_hallazgo { get; set; }
        public int num_hallazgo { get; set; }
    }
}
