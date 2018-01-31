using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{

    public class lc_acc_empresa_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_acc_empresa> Listar()
        {
            //DB = LocalDB.Instance;
            var lista = DB.lc_acc_empresa;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_acc_empresa> lista = Listar();
            //DB.lc_acc_empresa.Delete();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_acc_empresa.Delete(x => x.cod_empresa == empresa);
            }           

        }

        public void Insertar(lc_acc_empresa entidad)
        {
            DB.lc_acc_empresa.Add(entidad);
            DB.SaveChanges();
        }
    }
}
