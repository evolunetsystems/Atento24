using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_incidente_personal_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public List<lc_pro_incidente_personal> Listar()
        {
            var lista = DB.lc_pro_incidente_personal;
            return lista.ToList();
        }

        public void Eliminar()
        {
            //DB.lc_pro_incidente_personal.Delete(x => x.cod_empresa == "E00000"
            //                                     && x.cod_unidad == "01");
            List<lc_pro_incidente_personal> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                DB.lc_pro_incidente_personal.Delete(x => x.cod_empresa == lista[i].cod_empresa
                                                 && x.cod_unidad == lista[i].cod_unidad);
            }

        }

        public void EliminarUno(lc_pro_incidente_personal entidad)
        {
            //List<lc_pro_incidente_personal> lista = Listar();
            //for (int i = 0; i < lista.Count(); i++)
            //{
            //    DB.lc_pro_incidente_personal.Delete(x => x.cod_empresa == lista[i].cod_empresa
            //                                     && x.cod_unidad == lista[i].cod_unidad
            //                                     && x.cod_incidente == lista[i].cod_incidente
            //                                     && x.cod_personal == lista[i].cod_personal);
            //}

            DB.lc_pro_incidente_personal.Delete(x => x.cod_empresa == entidad.cod_empresa
                                                 && x.cod_unidad == entidad.cod_unidad
                                                 && x.cod_incidente == entidad.cod_incidente);

        }

        public void Insertar(lc_pro_incidente_personal entidad)
        {
            DB.lc_pro_incidente_personal.Add(entidad);
            DB.SaveChanges();
        }
    }
}
