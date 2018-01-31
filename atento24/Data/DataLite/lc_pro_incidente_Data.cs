using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_incidente_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_incidente entidad)
        {
            //DB.lc_pro_evidencia.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_incidente> Listar()
        {
            var lista = DB.lc_pro_incidente;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_incidente> lista = Listar();
            for (int i = 0; i < lista.Count; i++)
            {
                EliminarUno(lista[i], true);

                //var empresa = lista[i].cod_empresa;
                //var unidad = lista[i].cod_unidad;
                //DB.lc_pro_incidente.Delete(x => x.cod_empresa == empresa
                //                            && x.cod_unidad == unidad);

            }
        }

        public void EliminarUno(lc_pro_incidente entidad, bool eli_tarea)
        {
            lc_pro_evidencia_Data o_Data_Evi = new lc_pro_evidencia_Data();
            lc_pro_incidente_personal_Data o_Data_Par = new lc_pro_incidente_personal_Data();
            lc_pro_tarea_Data o_Data_Tar = new lc_pro_tarea_Data();

            //  Eliminar Evidencia
            List< lc_pro_evidencia> lst_evidencia = o_Data_Evi.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_referencia == entidad.cod_incidente).ToList();
            
            for (int i = 0; i < lst_evidencia.Count; i++)
            {
                o_Data_Evi.EliminarUno(lst_evidencia[i]);
            }

            //  Eliminar Participantes
            List<lc_pro_incidente_personal> lst_personal = o_Data_Par.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_incidente == entidad.cod_incidente).ToList();
            for (int i = 0; i < lst_personal.Count; i++)
            {
                o_Data_Par.EliminarUno(lst_personal[i]);
            }

            //Eliminar Tareas
            if (eli_tarea)
            {
                List<lc_pro_tarea> lst_tarea = o_Data_Tar.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                       && x.cod_unidad == entidad.cod_unidad
                                       && x.cod_referencia == entidad.cod_incidente).ToList();
                for (int i = 0; i < lst_tarea.Count; i++)
                {
                    o_Data_Tar.EliminarUno(lst_tarea[i], true);
                }
            }
            DB.lc_pro_incidente.Delete(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_incidente == entidad.cod_incidente);
        }

        public void InsertarSinc(lc_pro_incidente entidad)
        {
            DB.lc_pro_incidente.Add(entidad);
            DB.SaveChanges();
        }

        public void Insertar(lc_pro_incidente entidad)
        {
            DB.lc_pro_incidente.Add(entidad);
            for (int i = 0; i < entidad.lst_lc_pro_evidencia.Count; i++)
            {
                entidad.lst_lc_pro_evidencia[i].cod_referencia = entidad.cod_incidente;
                DB.lc_pro_evidencia.Add(entidad.lst_lc_pro_evidencia[i]);
            }

            for (int i = 0; i < entidad.lst_lc_pro_incidente_personal.Count; i++)
            {
                entidad.lst_lc_pro_incidente_personal[i].cod_incidente = entidad.cod_incidente;
                DB.lc_pro_incidente_personal.Add(entidad.lst_lc_pro_incidente_personal[i]);
            }
            DB.SaveChanges();
        }

        public void Modificar(lc_pro_incidente entidad)
        {
            EliminarUno(entidad, false);
            Insertar(entidad);
        }
    }
}
