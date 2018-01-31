namespace atento24.Data.ORM
{
    public class lc_acc_unidad : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string nom_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string nom_unidad { get; set; }
        public string sim_unidad { get; set; }

        public string cod_usuario { get; set; }
        public string cod_personal { get; set; }
        public string nom_personal { get; set; }
    }
}
