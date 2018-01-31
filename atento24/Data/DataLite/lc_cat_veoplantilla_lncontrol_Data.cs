using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_cat_veoplantilla_lncontrol_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_cat_veoplantilla_lncontrol> Listar()
        {
            var lista = DB.lc_cat_veoplantilla_lncontrol;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_cat_veoplantilla_lncontrol> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_cat_veoplantilla_lncontrol.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad);
            }

        }

        public void Insertar(lc_cat_veoplantilla_lncontrol entidad)
        {
            DB.lc_cat_veoplantilla_lncontrol.Add(entidad);
            DB.SaveChanges();
        }
    }
}
