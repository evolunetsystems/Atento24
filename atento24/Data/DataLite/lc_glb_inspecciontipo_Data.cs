using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_glb_inspecciontipo_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_glb_inspecciontipo> Listar()
        {
            var lista = DB.lc_glb_inspecciontipo;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_glb_inspecciontipo> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var s_codigo = lista[i].cod_inspecciontipo;
                DB.lc_glb_inspecciontipo.Delete(x => x.cod_inspecciontipo == s_codigo);
            }
        }

        public void Insertar(lc_glb_inspecciontipo entidad)
        {
            DB.lc_glb_inspecciontipo.Add(entidad);
            DB.SaveChanges();
        }
    }
}
