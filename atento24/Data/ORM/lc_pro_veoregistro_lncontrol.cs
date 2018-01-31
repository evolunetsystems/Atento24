namespace atento24.Data.ORM
{
    public class lc_pro_veoregistro_lncontrol : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public string cod_veoregistro { get; set; }

        public string cod_lncontrol { get; set; }
        public string cod_riesgo { get; set; }
        public int ord_lncontrol { get; set; }
        public int pes_lncontrol { get; set; }
        public int cum_lncontrol { get; set; }
        public int noc_lncontrol { get; set; }
        public int noa_lncontrol { get; set; }
        public string com_lncontrol { get; set; }

        public string nom_lncontrol { get; set; }
        public string nom_riesgo { get; set; }

        public bool b_cum_lncontrol { get; set; }
        public bool b_noc_lncontrol { get; set; }
        public bool b_noa_lncontrol { get; set; }

        public string cod_simbolo { get; set; }
        public int val_simbolo { get; set; }
        public string cod_medida { get; set; }
        public decimal pa1_lncontrol { get; set; }
        public decimal pa2_lncontrol { get; set; }
        public bool vis_lncontrol { get; set; }
        public string cod_tipodato { get; set; }
        public string nom_simbolo { get; set; }
        public bool ena_simbolo { get; set; }
        public decimal val_lncontrol { get; set; }
        public string ib_check { get; set; }
        public string ib_check_nc { get; set; }
        public string ib_check_na { get; set; }
        public string ale_lncontrol { get; set; }
    }
}
