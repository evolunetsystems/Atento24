using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_elimina_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_pro_elimina> Listar()
        {
            var lista = DB.lc_pro_elimina;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_elimina> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_pro_elimina.Delete(x => x.cod_empresa == empresa);
            }
        }

        public void Insertar(lc_pro_elimina entidad)
        {
            DB.lc_pro_elimina.Add(entidad);
            DB.SaveChanges();
        }
    }
}
