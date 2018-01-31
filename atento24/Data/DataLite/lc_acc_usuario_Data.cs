using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_acc_usuario_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public lc_acc_usuario ListarUno()
        {
            //DB = LocalDB.Instance;
            var usuario = DB.lc_acc_usuario;
            return usuario.FirstOrDefault();
        }

        public int CanRegistro()
        {
            int nReg = DB.lc_acc_usuario.Count();
            return nReg;
        }

        public void EliminarUno(lc_acc_usuario entidad)
        {
            DB.lc_acc_usuario.Delete(x => x.cod_usuario == entidad.cod_usuario);
        }

        public void Insertar(lc_acc_usuario entidad)
        {
            DB.lc_acc_usuario.Add(entidad);
            DB.SaveChanges();
        }

        public void Actualizar(lc_acc_usuario entidad)
        {
            EliminarUno(entidad);
            Insertar(entidad);            
        }
    }
}
