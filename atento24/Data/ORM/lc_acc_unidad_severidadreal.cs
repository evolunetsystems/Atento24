namespace atento24.Data.ORM
{
    public class lc_acc_unidad_severidadreal : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }
        public string cod_severidadreal { get; set; }
        public string nom_severidadreal { get; set; }
        public string com_severidadreal { get; set; }

        public string cod_sisgestion { get; set; }
        public string nom_sisgestion { get; set; }
        public string nom_unidad { get; set; }
    }
}
