using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_estado_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_pro_estado> Listar()
        {
            var lista = DB.lc_pro_estado;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_estado> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_pro_evidencia.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad);
            }
        }

        public void EliminarUno(lc_pro_estado entidad)
        {
            DB.lc_pro_estado.Delete(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_referencia == entidad.cod_referencia);
        }

        public void Insertar(lc_pro_estado entidad)
        {
            DB.lc_pro_estado.Add(entidad);
            DB.SaveChanges();
        }
    }
}
