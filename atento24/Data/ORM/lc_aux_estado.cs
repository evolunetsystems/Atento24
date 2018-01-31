namespace atento24.Data.ORM
{
    public class lc_aux_estado : lc_tabla_base
    {
        public string cod_empresa { get; set; }

        public string cod_estado { get; set; }
        public string nom_estado { get; set; }
        public string des_estado { get; set; }
        public string cod_modulo { get; set; }
        public string nom_modulo { get; set; }
        public string ver_opcion { get; set; }
        public int eta_estado { get; set; }
        public string cmb_estado { get; set; }
    }
}
