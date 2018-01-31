using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_glb_hallazgoclase_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_glb_hallazgoclase> Listar()
        {
            var lista = DB.lc_glb_hallazgoclase;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_glb_hallazgoclase> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var s_codigo = lista[i].cod_hallazgoclase;
                DB.lc_glb_hallazgoclase.Delete(x => x.cod_hallazgoclase == s_codigo);
            }
        }

        public void Insertar(lc_glb_hallazgoclase entidad)
        {
            DB.lc_glb_hallazgoclase.Add(entidad);
            DB.SaveChanges();
        }
    }
}
