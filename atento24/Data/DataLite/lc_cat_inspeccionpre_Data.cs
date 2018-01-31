using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_cat_inspeccionpre_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_cat_inspeccionpre> Listar()
        {
            var lista = DB.lc_cat_inspeccionpre;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_cat_inspeccionpre> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_cat_inspeccionpre.Delete(x => x.cod_empresa == empresa);
            }

        }

        public void Insertar(lc_cat_inspeccionpre entidad)
        {
            DB.lc_cat_inspeccionpre.Add(entidad);
            DB.SaveChanges();
        }
    }
}
