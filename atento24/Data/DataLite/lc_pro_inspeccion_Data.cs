using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_inspeccion_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_inspeccion entidad)
        {
            //DB.lc_pro_inspeccion.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_inspeccion> Listar()
        {
            var lista = DB.lc_pro_inspeccion;
            return lista.ToList();
        }

        public List<lc_pro_inspeccion> ListarUno(lc_pro_inspeccion entidad)
        {
            var lista = DB.lc_pro_inspeccion.Where(x => x.cod_empresa == entidad.cod_empresa
                                            && x.cod_unidad == entidad.cod_unidad
                                            && x.cod_inspeccion == entidad.cod_inspeccion);
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_inspeccion> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_pro_inspeccion.Delete(x => x.cod_empresa == empresa
                                            && x.cod_unidad == unidad);
            }
        }

        public void InsertarSinc(lc_pro_inspeccion entidad)
        {
            DB.lc_pro_inspeccion.Add(entidad);
            DB.SaveChanges();
        }

        public void Insertar(lc_pro_inspeccion entidad)
        {
            DB.lc_pro_inspeccion.Add(entidad);

            for (int i = 0; i < entidad.lst_lc_pro_participante.Count; i++)
            {
                entidad.lst_lc_pro_participante[i].cod_referencia = entidad.cod_inspeccion;
                DB.lc_pro_participante.Add(entidad.lst_lc_pro_participante[i]);
            }
            DB.SaveChanges();
        }

        public void EliminarUno(lc_pro_inspeccion entidad)
        {
            lc_pro_participante_Data o_Data_Par = new lc_pro_participante_Data();

            //  Eliminar Participantes
            for (int i = 0; i < entidad.lst_lc_pro_participante.Count; i++)
            {
                o_Data_Par.EliminarUno(entidad.lst_lc_pro_participante[i]);
            }

            //  Eliminando Inspección
            DB.lc_pro_inspeccion.Delete(x => x.cod_empresa == entidad.cod_empresa
                                        && x.cod_unidad == entidad.cod_unidad
                                        && x.cod_inspeccion == entidad.cod_inspeccion);

        }

        public void Modificar(lc_pro_inspeccion entidad)
        {
            EliminarUno(entidad);
            Insertar(entidad);
        }
    }
}
