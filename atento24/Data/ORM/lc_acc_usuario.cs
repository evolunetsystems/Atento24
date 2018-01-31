namespace atento24.Data.ORM
{
    public class lc_acc_usuario
    {
        public string cod_usuario { get; set; }
        public string nom_usuario { get; set; }
        public string cla_usuario { get; set; }
        public bool syn_global { get; set; }
        public bool syn_auxiliar { get; set; }
        public bool syn_catalogo { get; set; }
        public bool syn_procesos { get; set; }
        public string tip_tabla { get; set; }

        public string ing_usuario { get; set; }
        public int ult_ingreso { get; set; }

        public string cod_empresa { get; set; }
        public string nom_empresa { get; set; }
        public string cod_perfil { get; set; }
        public string nom_perfil { get; set; }
    }
}
