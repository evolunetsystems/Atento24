using System.Collections.Generic;

namespace atento24.Data.ORM
{
    public class lc_pro_tarea : lc_tabla_base
    {
        public string cod_empresa { get; set; }
        public string cod_unidad { get; set; }

        public string cod_tarea { get; set; }
        public string ini_tarea { get; set; }
        public string fin_tarea { get; set; }
        public string cod_periodo { get; set; }
        public string sol_personal { get; set; }
        public string eje_personal { get; set; }
        public string des_tarea { get; set; }
        public string cod_modulo { get; set; }
        public string cod_modulo_2do { get; set; }
        public string cod_referencia { get; set; }
        public string cod_tipoubicacion { get; set; }
        public string cod_labor { get; set; }
        public string cod_lugar { get; set; }
        public string cod_equipo { get; set; }

        public int dia_tarea { get; set; }
        public string nom_periodo { get; set; }
        public string nom_sol_personal { get; set; }
        public string nom_eje_personal { get; set; }
        public string nom_modulo { get; set; }
        public string nom_tipoubicacion { get; set; }
        public string nom_ubicacion { get; set; }
        public string nom_labor { get; set; }
        public string nom_lugar { get; set; }
        public string nom_equipo { get; set; }

        public decimal pro_avance { get; set; }
        public decimal por_avance { get; set; }
        public string des_avance { get; set; }
        public string cod_estado { get; set; }
        public string nom_estado { get; set; }
        public string des_origen { get; set; }

        public string cod_opcion { get; set; }
        public string nom_opcion { get; set; }
        public int can_opcion { get; set; }
        public string ver_opcion { get; set; }
        public string mis_opcion { get; set; }
        public string cod_personal { get; set; }
        public string com_avance { get; set; }
        public string btn_opcion { get; set; }
        public int eta_estado { get; set; }
        public bool sincronizado { get; set; }
        public string sincr_color { get; set; }

        /*  Propiedades para Botones de Lista de Tareas*/
        public bool coment_boton { get; set; }
        public bool ver_btnAtender { get; set; }
        public bool ena_boton { get; set; }

        public bool ver_comentario { get; set; }
        public int num_comentario { get; set; }

        public List<lc_pro_avance> lst_lc_pro_avance = new List<lc_pro_avance>();
        public List<lc_pro_evidencia> lst_lc_pro_evidencia = new List<lc_pro_evidencia>();
        public List<lc_pro_estado> lst_lc_pro_estado = new List<lc_pro_estado>();
        public string ret_padre { get; set; }
        public string ret_hijo { get; set; }
        public string titulo { get; set; }
        public string ret_titulo { get; set; }
        public bool ver_boton { get; set; }
    }    
}
