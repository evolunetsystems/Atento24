using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_cat_labor_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_cat_labor> Listar()
        {
            var lista = DB.lc_cat_labor;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_cat_labor> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_cat_labor.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad);
            }

        }

        public void Insertar(lc_cat_labor entidad)
        {
            DB.lc_cat_labor.Add(entidad);
            DB.SaveChanges();
        }
    }
}
