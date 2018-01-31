using atento24.Data.DataLite;
using atento24.Data.LiteConnection;
using atento24.Data.StandarDB;
using atento24.Pages;
using atento24.Recursos;
using Xamarin.Forms;

namespace atento24
{
    public partial class App : Application
    {
        public static LocalDB DB { get; private set; }

        public App()
        {
            InitializeComponent();
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            DefinirRuta();
        }

        private void DefinirRuta()
        {
            //SQlite
            Keys.DataBaseName = "prueba.db";
            lc_acc_usuario_Data o_Data = new lc_acc_usuario_Data();

            if (o_Data.CanRegistro() == 0)
            {
                MainPage = new MainPage("ok");
            }
            else
            {
                //  Validamos Sincronizacion.
                var v_usuario = o_Data.ListarUno();
                VarGlobal.cod_usuario = v_usuario.cod_usuario;
                VarGlobal.ip = "App";
                VarGlobal.alerta_registro = false;
                if (v_usuario.syn_auxiliar)
                {
                    //  Direccionar a Seleccionar Empresa
                    MainPage = new MasterDetailPage1("pg_empresa");
                }
                else
                {
                    //  Direccionar a Sincronizar
                    MainPage = new MasterDetailPage1("pg_sincronizar");
                }
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
