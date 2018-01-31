using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_hallazgo_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_hallazgo entidad)
        {
            //DB.lc_pro_hallazgo.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_hallazgo> Listar()
        {
            var lista = DB.lc_pro_hallazgo;
            return lista.ToList();
        }

        public List<lc_pro_hallazgo> ListarUno(lc_pro_hallazgo entidad)
        {
            var lista = DB.lc_pro_hallazgo.Where(x => x.cod_empresa == entidad.cod_empresa
                                            && x.cod_unidad == entidad.cod_unidad
                                            && x.cod_hallazgo == entidad.cod_hallazgo);
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_hallazgo> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                EliminarUno(lista[i], true);
            }
        }

        public void InsertarSinc(lc_pro_hallazgo entidad)
        {
            entidad.niv_color = entidad.nom_tblnivelriesgo == "ALTO" ? "#FF0000" : (entidad.nom_tblnivelriesgo == "MEDIO" ? "#FF8000" : "#FFBF00");
            DB.lc_pro_hallazgo.Add(entidad);

            DB.SaveChanges();
        }

        public void Insertar(lc_pro_hallazgo entidad)
        {
            entidad.niv_color = entidad.nom_tblnivelriesgo == "ALTO" ? "#FF0000" : (entidad.nom_tblnivelriesgo == "MEDIO" ? "#FF8000" : "#FFBF00") ;
            DB.lc_pro_hallazgo.Add(entidad);
            for (int i = 0; i < entidad.lst_lc_pro_evidencia.Count; i++)
            {
                entidad.lst_lc_pro_evidencia[i].cod_referencia = entidad.cod_hallazgo;
                DB.lc_pro_evidencia.Add(entidad.lst_lc_pro_evidencia[i]);
            }

            for (int i = 0; i < entidad.lst_lc_pro_coordenada.Count; i++)
            {
                entidad.lst_lc_pro_coordenada[i].cod_referencia = entidad.cod_hallazgo;
                DB.lc_pro_coordenada.Add(entidad.lst_lc_pro_coordenada[i]);
            }

            for (int i = 0; i < entidad.lst_lc_pro_participante.Count; i++)
            {
                entidad.lst_lc_pro_participante[i].cod_referencia = entidad.cod_hallazgo;
                DB.lc_pro_participante.Add(entidad.lst_lc_pro_participante[i]);
            }

            for (int i = 0; i < entidad.lst_lc_pro_tarea.Count; i++)
            {
                entidad.lst_lc_pro_tarea[i].cod_referencia = entidad.cod_hallazgo;
                DB.lc_pro_tarea.Add(entidad.lst_lc_pro_tarea[i]);
            }
            DB.SaveChanges();
        }

        public void EliminarUno(lc_pro_hallazgo entidad, bool eli_tarea)
        {
            lc_pro_coordenada_Data o_Data_Coo = new lc_pro_coordenada_Data();
            lc_pro_evidencia_Data o_Data_Evi = new lc_pro_evidencia_Data();
            lc_pro_participante_Data o_Data_Par = new lc_pro_participante_Data();
            lc_pro_tarea_Data o_Data_Tar = new lc_pro_tarea_Data();

            //  Eliminar Coordenadas
            List<lc_pro_coordenada> lst_coordenada = o_Data_Coo.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_referencia == entidad.cod_hallazgo).ToList();
            for (int i = 0; i < lst_coordenada.Count; i++)
            {
                o_Data_Coo.EliminarUno(lst_coordenada[i]);
            }

            //  Eliminar Evidencia
            List<lc_pro_evidencia> lst_evidencia = o_Data_Evi.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_referencia == entidad.cod_hallazgo).ToList();

            for (int i = 0; i < lst_evidencia.Count; i++)
            {
                o_Data_Evi.EliminarUno(lst_evidencia[i]);
            }

            //  Eliminar Participantes
            List<lc_pro_participante> lst_participante = o_Data_Par.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_referencia == entidad.cod_hallazgo).ToList();
            for (int i = 0; i < lst_participante.Count; i++)
            {
                o_Data_Par.EliminarUno(lst_participante[i]);
            }

            //  Eliminar Tareas
            if (eli_tarea)
            {
                List<lc_pro_tarea> lst_tarea = o_Data_Tar.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                  && x.cod_unidad == entidad.cod_unidad
                                  && x.cod_referencia == entidad.cod_hallazgo).ToList();
                for (int i = 0; i < lst_tarea.Count; i++)
                {
                    o_Data_Tar.EliminarUno(lst_tarea[i], true);
                }
            }
               

            //  Eliminando Hallazgo
            DB.lc_pro_hallazgo.Delete(x => x.cod_empresa == entidad.cod_empresa
                                        && x.cod_unidad == entidad.cod_unidad
                                        && x.cod_hallazgo == entidad.cod_hallazgo);

        }

        public void Modificar(lc_pro_hallazgo entidad)
        {
            EliminarUno(entidad, false);
            Insertar(entidad);
            string s_cod_referencia = entidad.cod_referencia;
            if (s_cod_referencia.Length > 0)
            {
                switch (entidad.cod_modulo)
                {
                    case "IP":
                        lc_pro_inspeccion_Data o_Data_Inp = new lc_pro_inspeccion_Data();
                        lc_pro_inspeccion ent_inspeccion = o_Data_Inp.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                                                            && x.cod_unidad == entidad.cod_unidad
                                                                            && x.cod_inspeccion == entidad.cod_referencia).FirstOrDefault();
                        if (ent_inspeccion != null)
                        {
                            ent_inspeccion.sincronizado = false;
                            o_Data_Inp.Modificar(ent_inspeccion);
                        }
                       
                        break;
                }
            }
        }
    }
}
