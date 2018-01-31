namespace atento24.Data.ORM
{
    public class lc_pro_coordenada : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_referencia { get; set; }

        public int num_coordenada { get; set; }
        public string cod_modulo { get; set; }
        public decimal lat_coordenada { get; set; }
        public decimal lon_coordenada { get; set; }
        public string com_coordenada { get; set; }
        public bool sincronizado { get; set; }
    }
}
