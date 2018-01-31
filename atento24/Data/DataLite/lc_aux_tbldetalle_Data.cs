using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_aux_tbldetalle_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_aux_tbldetalle> Listar()
        {
            var lista = DB.lc_aux_tbldetalle;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_aux_tbldetalle> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                DB.lc_aux_tbldetalle.Delete(x => x.cod_empresa == empresa);
            }
        }

        public void Insertar(lc_aux_tbldetalle entidad)
        {
            DB.lc_aux_tbldetalle.Add(entidad);
            DB.SaveChanges();
        }
    }
}
