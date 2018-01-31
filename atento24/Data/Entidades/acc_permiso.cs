using atento24.Pages;
using System;

namespace atento24.Data.Entidades
{
    public class acc_permiso
    {
        public string cod_empresa { get; set; }
        public string cod_menu { get; set; }
        public string cod_menupadre { get; set; }
        public string nom_menu { get; set; }
        public string sen_menu { get; set; }
        public int niv_menu { get; set; }
        public string ico_menu { get; set; }

        public string cod_perfil { get; set; }
        public string opc_ver { get; set; }
        public string opc_nuevo { get; set; }
        public string opc_modificar { get; set; }
        public string opc_imprimir { get; set; }
        public string opc_eliminar { get; set; }
        public bool acceso { get; set; }
        public string salir { get; set; }

        public acc_permiso()
        {
            TargetType = typeof(MasterDetailPage1Detail);
        }
        public Type TargetType { get; set; }


    }
}
