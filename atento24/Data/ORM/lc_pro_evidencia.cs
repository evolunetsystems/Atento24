namespace atento24.Data.ORM
{
    public class lc_pro_evidencia : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_referencia { get; set; }

        public int num_evidencia { get; set; }
        public string fec_evidencia { get; set; }
        public byte[] dat_evidencia { get; set; }
        public string nom_evidencia { get; set; }
        public string tip_evidencia { get; set; }
        public decimal tam_evidencia { get; set; }
        public string com_evidencia { get; set; }
        public string cod_modulo { get; set; }
        public string tip_etapa { get; set; }
        public int num_etapa { get; set; }

        public string nom_modulo { get; set; }
        public bool sincronizado { get; set; }
    }
}
