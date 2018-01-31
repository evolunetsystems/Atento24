using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_acc_unidad_sisgestion_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_acc_unidad_sisgestion> Listar()
        {
            var lista = DB.lc_acc_unidad_sisgestion;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_acc_unidad_sisgestion> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_acc_unidad_sisgestion.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad);
            }
        }

        public void Insertar(lc_acc_unidad_sisgestion entidad)
        {
            DB.lc_acc_unidad_sisgestion.Add(entidad);
            DB.SaveChanges();
        }
    }
}
