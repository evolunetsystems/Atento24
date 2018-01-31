using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_acc_unidad_tipoubicacion_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_acc_unidad_tipoubicacion> Listar()
        {
            var lista = DB.lc_acc_unidad_tipoubicacion;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_acc_unidad_tipoubicacion> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_acc_unidad_tipoubicacion.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad);
            }
        }

        public void Insertar(lc_acc_unidad_tipoubicacion entidad)
        {
            DB.lc_acc_unidad_tipoubicacion.Add(entidad);
            DB.SaveChanges();
        }
    }
}
