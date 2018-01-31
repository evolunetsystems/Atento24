using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Data.StandarDB;
using atento24.Pages;
using atento24.Pages.Popup;
using atento24.Recursos;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static LocalDB DB { get; private set; }
        public pg_Loading loadingPage = new pg_Loading();
        public MainPage(string s_cod)
        {
            InitializeComponent();

            OnBackButtonPressed();
            BindingContext = this;
            IsBusy = false;
            if (s_cod == "error")
            {
                MostrarMensaje();
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void MostrarMensaje()
        {
            await DisplayAlert("Acceso", "Usuario o Contraseña incorrecta", "Aceptar");
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void Button_Login(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            //var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            string s_usu = EmailEntry.Text;
            string s_clave = PasswordEntry.Text;

            DB = LocalDB.Instance;

            // Validamos credenciales
            bool b_res = await ValidarControles();
            if (!b_res)
            {
                lc_acc_usuario o_Entidad = new lc_acc_usuario()
                {
                    cod_usuario = s_usu,
                    cla_usuario = s_clave
                };


                bool b_user = ValidarUsuario(o_Entidad);

                if (!b_user)
                {
                    //  Si Ususario no existe en la Nube
                    VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                    {
                        tip_mensaje = "ERR",
                        tit_mensaje = "Error de validación",
                        tex_mensaje = "Usuario o Contraseña incorrecta"
                    });
                    //await DisplayAlert("Acceso", "Usuario o Contraseña incorrecta", "Aceptar");
                    EmailEntry.Text = "";
                    PasswordEntry.Text = "";
                    Content.IsEnabled = true;
                }
                else
                {
                    // Eliminamos en SQLite
                    lc_acc_usuario_Data o_Data = new lc_acc_usuario_Data();
                    o_Data.EliminarUno(o_Entidad);
                    o_Data.Insertar(o_Entidad);

                    await Navigation.PushModalAsync(new pg_sincronizar());
                }
            }

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
        }

        private async Task<bool> ValidarControles()
        {
            bool b_error = false;
            string s_msg = "";

            if (string.IsNullOrEmpty(EmailEntry.Text))
            {
                s_msg = "Debe Ingresar un usuario";
                b_error = true;
                EmailEntry.Focus();
            }

            if (string.IsNullOrEmpty(PasswordEntry.Text) && !b_error)
            {
                s_msg = "Debe Ingresar una clave";
                b_error = true;
                PasswordEntry.Focus();
            }

            if (b_error)
            {
                await DisplayAlert("Error", s_msg, "Aceptar");
            }
            return b_error;
        }

        public bool ValidarUsuario(lc_acc_usuario entidad)
        {
            bool b_result = false;

            if (VerificarConexion())
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://webapi.atento24.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/acc_usuario", content).Result;
                //IsLoading = false;
                if (response.IsSuccessStatusCode)
                {
                    lc_acc_usuario ent_usuario = JsonConvert.DeserializeObject<lc_acc_usuario>(response.Content.ReadAsStringAsync().Result);
                    if (ent_usuario.cod_usuario == null)
                    {
                        //await DisplayAlert("Acceso", "Usuario o Contraseña incorrecta", "Aceptar");
                        EmailEntry.Text = "";
                        PasswordEntry.Text = "";
                        Content.IsEnabled = true;
                        //return;
                    }
                    else
                    {
                        VarGlobal.cod_usuario = ent_usuario.cod_usuario.Trim();
                        //VarGlobal.nom_usuario = ent_usuario.nom_perfil.Trim();
                        VarGlobal.cod_empresa = "";
                        VarGlobal.cod_unidad = "";
                        b_result = true;
                    }
                }
            }
            else
            {
                MostrarPopup();

            }


            return b_result;
        }

        private async void MostrarPopup()
        {
            await Navigation.RemovePopupPageAsync(loadingPage);
            VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
            {
                tip_mensaje = "ERR",
                tit_mensaje = "Error de Conexión",
                tex_mensaje = "Verifica tu conexión a internet"
            });
            await Navigation.PushPopupAsync(VarGlobal._mensaje);
        }

        private bool VerificarConexion()
        {
            bool b_conexion = true;
            if (!CrossConnectivity.Current.IsConnected)
            {
                b_conexion = false;
            }
            return b_conexion;
        }


    }

}
