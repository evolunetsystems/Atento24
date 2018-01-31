using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_cat_ocurrencia_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_cat_ocurrencia> Listar()
        {
            var lista = DB.lc_cat_ocurrencia;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_cat_ocurrencia> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_cat_ocurrencia.Delete(x => x.cod_empresa == empresa);
            }

        }

        public void Insertar(lc_cat_ocurrencia entidad)
        {
            DB.lc_cat_ocurrencia.Add(entidad);
            DB.SaveChanges();
        }
    }
}
