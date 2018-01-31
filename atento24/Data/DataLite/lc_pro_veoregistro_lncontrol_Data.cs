using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_veoregistro_lncontrol_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_pro_veoregistro_lncontrol> Listar()
        {
            var lista = DB.lc_pro_veoregistro_lncontrol;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_veoregistro_lncontrol> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                //var veoregistro = lista[i].cod_veoregistro;  && x.cod_veoregistro == veoregistro
                DB.lc_pro_veoregistro_lncontrol.Delete(x => x.cod_empresa == empresa
                                            && x.cod_unidad == unidad);
            }
        }

        public void EliminarUno(lc_pro_veoregistro_lncontrol entidad)
        {

            DB.lc_pro_veoregistro_lncontrol.Delete(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_veoregistro == entidad.cod_veoregistro);
        }

        public void Insertar(lc_pro_veoregistro_lncontrol entidad)
        {
            DB.lc_pro_veoregistro_lncontrol.Add(entidad);
            DB.SaveChanges();
        }
    }
}
