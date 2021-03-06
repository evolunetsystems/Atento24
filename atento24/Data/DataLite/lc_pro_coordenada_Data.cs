﻿using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_coordenada_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_coordenada entidad)
        {
            //DB.lc_pro_coordenada.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_coordenada> Listar()
        {
            var lista = DB.lc_pro_coordenada;
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_coordenada> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                var empresa = lista[i].cod_empresa;
                var unidad = lista[i].cod_unidad;
                DB.lc_pro_coordenada.Delete(x => x.cod_empresa == empresa && x.cod_unidad == unidad );
            }
        }
        
        public void EliminarUno(lc_pro_coordenada entidad)
        {

            DB.lc_pro_coordenada.Delete(x => x.cod_empresa == entidad.cod_empresa
                                    && x.cod_unidad == entidad.cod_unidad
                                    && x.cod_referencia == entidad.cod_referencia);
        }

        public void Insertar(lc_pro_coordenada entidad)
        {
            DB.lc_pro_coordenada.Add(entidad);
            DB.SaveChanges();
        }
    }
}
