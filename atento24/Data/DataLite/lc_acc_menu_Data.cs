using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_acc_menu_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_acc_menu> Listar()
        {
            var lista = DB.lc_acc_menu;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_acc_menu> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_acc_menu.Delete(x => x.cod_empresa == empresa);
            }
        }

        public void Insertar(lc_acc_menu entidad)
        {
            DB.lc_acc_menu.Add(entidad);
            DB.SaveChanges();
        }
    }
}
