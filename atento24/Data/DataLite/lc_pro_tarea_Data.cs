using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_tarea_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_tarea entidad)
        {
            //DB.lc_pro_tarea.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_tarea> Listar()
        {
            var lista = DB.lc_pro_tarea;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_tarea> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                EliminarUno(lista[i], true);

                //var empresa = lista[i].cod_empresa;
                //var unidad = lista[i].cod_unidad;
                //var referencia = lista[i].cod_referencia;
                //DB.lc_pro_tarea.Delete(x => x.cod_empresa == empresa 
                //                         && x.cod_unidad == unidad 
                //                         && x.cod_referencia == referencia);
            }
        }

        /// <summary>
        /// Metodo para eliminar una tarea
        /// </summary>
        /// <param name="entidad">datos de la tarea a eliminar</param>
        /// <param name="b_todo">indica si eliminas avances, estado y evidencias</param>
        public void EliminarUno(lc_pro_tarea entidad, bool b_todo)
        {
            lc_pro_evidencia_Data o_Data_Evi = new lc_pro_evidencia_Data();
            lc_pro_avance_Data o_Data_Avc = new lc_pro_avance_Data();
            lc_pro_estado_Data o_Data_Est = new lc_pro_estado_Data();

            if (b_todo)
            {
                //  Eliminar Evidencia
                List<lc_pro_evidencia> lst_evidencia = o_Data_Evi.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                       && x.cod_unidad == entidad.cod_unidad
                                       && x.cod_referencia == entidad.cod_tarea).ToList();

                for (int i = 0; i < lst_evidencia.Count; i++)
                {
                    o_Data_Evi.EliminarUno(lst_evidencia[i]);
                }

                //  Eliminar Avance
                List<lc_pro_avance> lst_avance = o_Data_Avc.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                       && x.cod_unidad == entidad.cod_unidad
                                       && x.cod_referencia == entidad.cod_tarea).ToList();

                for (int i = 0; i < lst_avance.Count; i++)
                {
                    o_Data_Avc.EliminarUno(lst_avance[i]);
                }

                //  Eliminar Estado
                List<lc_pro_estado> lst_estado = o_Data_Est.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                       && x.cod_unidad == entidad.cod_unidad
                                       && x.cod_referencia == entidad.cod_tarea).ToList();

                for (int i = 0; i < lst_estado.Count; i++)
                {
                    o_Data_Est.EliminarUno(lst_estado[i]);
                }
            }
            

            DB.lc_pro_tarea.Delete(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_tarea == entidad.cod_tarea);
        }

        public void Insertar(lc_pro_tarea entidad)
        {
            DB.lc_pro_tarea.Add(entidad);
            DB.SaveChanges();
        }

        public void Modificar(lc_pro_tarea entidad)
        {
            EliminarUno(entidad, false);
            Insertar(entidad);
            string s_cod_referencia = entidad.cod_referencia.Trim();
            if (s_cod_referencia.Length > 0)
            {
                switch (entidad.cod_modulo)
                {
                    case "HL":
                        lc_pro_hallazgo_Data o_Data_Hall = new lc_pro_hallazgo_Data();
                        lc_pro_hallazgo ent_hallazgo = o_Data_Hall.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                                                            && x.cod_unidad == entidad.cod_unidad
                                                                            && x.cod_hallazgo == entidad.cod_referencia).FirstOrDefault();
                        if (ent_hallazgo != null)
                        {
                            ent_hallazgo.sincronizado = false;
                            o_Data_Hall.Modificar(ent_hallazgo);
                        }
                        
                        break;
                    case "IN":
                        lc_pro_incidente_Data o_Data_Inc = new lc_pro_incidente_Data();
                        lc_pro_incidente ent_incidente = o_Data_Inc.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                                                            && x.cod_unidad == entidad.cod_unidad
                                                                            && x.cod_incidente == entidad.cod_referencia).FirstOrDefault();
                        if (ent_incidente != null)
                        {
                            ent_incidente.sincronizado = false;
                            o_Data_Inc.Modificar(ent_incidente);
                        }
                        
                        break;
                }
            }
        }
    }
}
