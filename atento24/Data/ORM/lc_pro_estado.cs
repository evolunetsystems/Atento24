namespace atento24.Data.ORM
{
    public class lc_pro_estado : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public int num_estado { get; set; }
        public string fec_estado { get; set; }
        public string cod_personal { get; set; }
        public string cod_estado { get; set; }
        public string nom_estado { get; set; }
        public string des_estado { get; set; }
        public string cod_modulo { get; set; }
        public string cod_referencia { get; set; }

        public string nom_personal { get; set; }
        public string nom_modulo { get; set; }
        public bool sincronizado { get; set; }
    }
}
