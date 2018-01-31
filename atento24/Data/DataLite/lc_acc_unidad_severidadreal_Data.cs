using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_acc_unidad_severidadreal_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_acc_unidad_severidadreal> Listar()
        {
            var lista = DB.lc_aux_severidadreal;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_acc_unidad_severidadreal> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_aux_severidadreal.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad);
            }
        }

        public void Insertar(lc_acc_unidad_severidadreal entidad)
        {
            DB.lc_aux_severidadreal.Add(entidad);
            DB.SaveChanges();
        }
    }
}
