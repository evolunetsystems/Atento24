namespace atento24.Data.ORM
{
    public class lc_pro_incidente_personal : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_incidente { get; set; }
        public string cod_personal { get; set; }
        public string dgn_personal { get; set; }
        public string tra_personal { get; set; }
        public int dpe_personal { get; set; }

        public string nom_personal { get; set; }
        public bool sincronizado { get; set; }
    }
}
