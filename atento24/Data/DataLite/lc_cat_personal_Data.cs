using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_cat_personal_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_cat_personal> Listar()
        {
            var lista = DB.lc_cat_personal;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_cat_personal> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_cat_personal.Delete(x => x.cod_empresa == empresa);
            }

        }

        public void Insertar(lc_cat_personal entidad)
        {
            DB.lc_cat_personal.Add(entidad);
            DB.SaveChanges();
        }
    }
}
