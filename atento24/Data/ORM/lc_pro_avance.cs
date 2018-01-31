namespace atento24.Data.ORM
{
    public class lc_pro_avance : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public int num_avance { get; set; }
        public string fec_avance { get; set; }
        public string cod_personal { get; set; }
        public int por_avance { get; set; }
        public string des_avance { get; set; }
        public string cod_modulo { get; set; }
        public string cod_referencia { get; set; }

        public string nom_personal { get; set; }
        public string nom_modulo { get; set; }
        public string tip_avance { get; set; }
        public string vpor_avance { get; set; }
        public bool sincronizado { get; set; }
    }
}
