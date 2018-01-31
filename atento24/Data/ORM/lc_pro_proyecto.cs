namespace atento24.Data.ORM
{
    public class lc_pro_proyecto : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public string cod_proyecto { get; set; }
        public string nom_proyecto { get; set; }
        public string des_proyecto { get; set; }
        public string dep_proyecto { get; set; }
        public int ord_proyecto { get; set; }

        public string cod_personal { get; set; }
        public string nom_personal { get; set; }

        public string cod_estado { get; set; }
        public string nom_estado { get; set; }
        public int por_avance { get; set; }
        public decimal dpor_avance { get; set; }

        public int can_reg { get; set; }
    }
}
