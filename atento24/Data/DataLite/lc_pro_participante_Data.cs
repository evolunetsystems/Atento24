using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_participante_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_participante entidad)
        {
            //DB.lc_pro_participante.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_participante> Listar()
        {
            var lista = DB.lc_pro_participante;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_participante> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                var referencia = lista[i].cod_referencia;
                DB.lc_pro_participante.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad && x.cod_referencia == referencia);
            }
        }

        public void EliminarUno(lc_pro_participante entidad)
        {

            DB.lc_pro_participante.Delete(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_referencia == entidad.cod_referencia);
        }

        public void Insertar(lc_pro_participante entidad)
        {
            DB.lc_pro_participante.Add(entidad);
            DB.SaveChanges();
        }
    }
}
