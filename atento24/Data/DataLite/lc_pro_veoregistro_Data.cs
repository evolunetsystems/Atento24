using atento24.Data.ORM;
using atento24.Data.StandarDB;
using System.Collections.Generic;
using System.Linq;

namespace atento24.Data.DataLite
{
    public class lc_pro_veoregistro_Data
    {
        private static LocalDB DB = LocalDB.Instance;

        public void Actualizar(lc_pro_veoregistro entidad)
        {
            //DB.lc_pro_veoregistro.(x => x.cod_usuario == entidad.cod_usuario);
        }

        public List<lc_pro_veoregistro> Listar()
        {
            var lista = DB.lc_pro_veoregistro;
            return lista.ToList();
        }

        public List<lc_pro_veoregistro> ListarUno(lc_pro_veoregistro entidad)
        {
            var lista = DB.lc_pro_veoregistro.Where(x => x.cod_empresa == entidad.cod_empresa
                                            && x.cod_unidad == entidad.cod_unidad
                                            && x.cod_veoregistro == entidad.cod_veoregistro);
            return lista.ToList();
        }

        public void Eliminar()
        {
            List<lc_pro_veoregistro> lista = Listar();
            for (int i = 0; i < lista.Count(); i++)
            {
                EliminarUno(lista[i]);

                //var empresa = lista[i].cod_empresa;
                //var unidad = lista[i].cod_unidad;
                //var veoregistro = lista[i].cod_veoregistro;
                //DB.lc_pro_veoregistro.Delete(x => x.cod_empresa == empresa
                //                            && x.cod_unidad == unidad
                //                            && x.cod_veoregistro == veoregistro);
            }
        }

        public void Insertar(lc_pro_veoregistro entidad)
        {            
            DB.lc_pro_veoregistro.Add(entidad);

            for (int i = 0; i < entidad.lst_lc_pro_coordenada.Count; i++)
            {
                entidad.lst_lc_pro_coordenada[i].cod_referencia = entidad.cod_veoregistro;
                DB.lc_pro_coordenada.Add(entidad.lst_lc_pro_coordenada[i]);
            }

            for (int i = 0; i < entidad.lst_lc_pro_participante.Count; i++)
            {
                entidad.lst_lc_pro_participante[i].cod_referencia = entidad.cod_veoregistro;
                DB.lc_pro_participante.Add(entidad.lst_lc_pro_participante[i]);
            }

            for (int i = 0; i < entidad.lst_lc_pro_veoregistro_lncontrol.Count; i++)
            {
                entidad.lst_lc_pro_veoregistro_lncontrol[i].cod_veoregistro = entidad.cod_veoregistro;
                DB.lc_pro_veoregistro_lncontrol.Add(entidad.lst_lc_pro_veoregistro_lncontrol[i]);
            }
            DB.SaveChanges();
        }

        public void InsertarSinc(lc_pro_veoregistro entidad)
        {            
            DB.lc_pro_veoregistro.Add(entidad);
            DB.SaveChanges();
        }

        public void EliminarUno(lc_pro_veoregistro entidad)
        {
            lc_pro_coordenada_Data o_Data_Coo = new lc_pro_coordenada_Data();
            lc_pro_participante_Data o_Data_Par = new lc_pro_participante_Data();
            lc_pro_veoregistro_lncontrol_Data o_Data_lnc = new lc_pro_veoregistro_lncontrol_Data();

            //  Eliminar Coordenadas
            List<lc_pro_coordenada> lst_coordenada = o_Data_Coo.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_referencia == entidad.cod_veoregistro).ToList();
            for (int i = 0; i < lst_coordenada.Count; i++)
            {
                o_Data_Coo.EliminarUno(lst_coordenada[i]);
            }

            //  Eliminar Participantes
            List<lc_pro_participante> lst_participante = o_Data_Par.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_referencia == entidad.cod_veoregistro).ToList();
            for (int i = 0; i < lst_participante.Count; i++)
            {
                o_Data_Par.EliminarUno(lst_participante[i]);
            }

            //  Eliminar LineaControl
            List<lc_pro_veoregistro_lncontrol> lst_lncontrol = o_Data_lnc.Listar().Where(x => x.cod_empresa == entidad.cod_empresa
                                   && x.cod_unidad == entidad.cod_unidad
                                   && x.cod_veoregistro == entidad.cod_veoregistro).ToList();
            for (int i = 0; i < lst_lncontrol.Count; i++)
            {
                o_Data_lnc.EliminarUno(lst_lncontrol[i]);
            }

            //  Eliminando VEO
            DB.lc_pro_veoregistro.Delete(x => x.cod_empresa == entidad.cod_empresa
                                        && x.cod_unidad == entidad.cod_unidad
                                        && x.cod_veoregistro == entidad.cod_veoregistro);

        }

        public void Modificar(lc_pro_veoregistro entidad)
        {
            EliminarUno(entidad);
            Insertar(entidad);
        }
    }
}
