namespace atento24.Data.ORM
{
    public class lc_pro_participante : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_referencia { get; set; }

        public string tip_participante { get; set; }
        public string cod_personal { get; set; }
        public string des_participante { get; set; }
        public string nom_personal { get; set; }
        public bool sincronizado { get; set; }
        public string cod_modulo { get; set; }
    }
}
