using System;
using System.Collections.Generic;
using System.IO;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Pages.Procesos;
using atento24.Recursos;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Busquedas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_evidencia : ContentPage
    {
        public pg_evidencia()
        {
            InitializeComponent();
            OnBackButtonPressed();
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            // Al devolver TRUE y no llamar a base, cancelamos el botón de retroceso de hardware :)
            // base.OnBackButtonPressed();
            return true;
        }

        private async void btnsalir_Clicked(object sender, EventArgs e)
        {
            switch (VarGlobal.cod_modulo_ret)
            {
                case "HL":
                    await Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                    break;
                case "IN":
                    await Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
                    break;
            }
        }

        private async void btngrabar_Clicked(object sender, EventArgs e)
        {
            if (!ValidarEvidencia())
            {

                switch (VarGlobal.cod_modulo_ret)
                {
                    case "HL":
                        VarGlobal.pro_hallazgo.lst_lc_pro_evidencia.Add(new lc_pro_evidencia()
                        {
                            cod_empresa = VarGlobal.cod_empresa,
                            cod_unidad = VarGlobal.cod_unidad,
                            cod_referencia = VarGlobal.pro_hallazgo.cod_hallazgo,
                            cod_modulo = VarGlobal.pro_hallazgo.cod_modulo,
                            fec_evidencia = DateTime.Now.ToString("dd/MM/yyyy"),
                            dat_evidencia = VarGlobal.dat_evidencia,
                            nom_evidencia = "evidencia.jpg",
                            tip_evidencia = VarGlobal.tip_evidencia,
                            tam_evidencia = VarGlobal.tam_evidencia,
                            com_evidencia = etEvidencia.Text,
                            tip_etapa = VarGlobal.tip_etapa,
                            num_etapa = VarGlobal.num_etapa,
                            sincronizado = false,
                            usuario = VarGlobal.cod_usuario,
                            ip = VarGlobal.ip,
                            estado = "A",
                            comando = "INS"
                        });
                        await Navigation.PushAsync(new pg_pro_hallazgo_mnt("B") { Title = VarGlobal.pro_hallazgo.titulo });
                        break;
                    case "IN":
                        VarGlobal.pro_incidente.lst_lc_pro_evidencia.Add(new lc_pro_evidencia()
                        {
                            cod_empresa = VarGlobal.cod_empresa,
                            cod_unidad = VarGlobal.cod_unidad,
                            cod_referencia = VarGlobal.pro_incidente.cod_incidente,
                            cod_modulo = "IN",
                            fec_evidencia = DateTime.Now.ToString("dd/MM/yyyy"),
                            dat_evidencia = VarGlobal.dat_evidencia,
                            nom_evidencia = "evidencia.jpg",
                            tip_evidencia = VarGlobal.tip_evidencia,
                            tam_evidencia = VarGlobal.tam_evidencia,
                            com_evidencia = etEvidencia.Text,
                            tip_etapa = VarGlobal.tip_etapa,
                            num_etapa = VarGlobal.num_etapa,
                            sincronizado = false,
                            usuario = VarGlobal.cod_usuario,
                            ip = VarGlobal.ip,
                            estado = "A",
                            comando = "INS"
                        });
                        await Navigation.PushAsync(new pg_pro_incidente_mnt("B") { Title = VarGlobal.pro_incidente.titulo });
                        break;
                }
            }
        }

        private bool ValidarEvidencia()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(etEvidencia.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Comentario...";
            }

            if (img_foto.Source == null && !b_Error)
            {
                b_Error = true;
                s_Mensaje = "Capturar Foto...";
            }

            if (b_Error)
            {
                VarGlobal._mensaje = new pg_mensaje(new ent_mensaje
                {
                    tip_mensaje = "ERR",
                    tit_mensaje = "Error de validación",
                    tex_mensaje = s_Mensaje
                });
                Navigation.PushPopupAsync(VarGlobal._mensaje);
            }

            return b_Error;
        }

        #region Foto
        private async void btnGal_Clicked(object sender, EventArgs e)
        {
            if (CrossMedia.Current.IsTakePhotoSupported)
            {
                var imagen = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small
                });

                if (imagen != null)
                {
                    Stream s_imagen = imagen.GetStream();
                    using (var memoryStream = new MemoryStream())
                    {
                        imagen.GetStream().CopyTo(memoryStream);
                        VarGlobal.dat_evidencia = memoryStream.ToArray();
                        VarGlobal.tam_evidencia = s_imagen.Length;
                        VarGlobal.tip_evidencia = "image/jpeg";
                    }

                    img_foto.Source = ImageSource.FromStream(() =>
                    {
                        var stream = imagen.GetStream();
                        imagen.Dispose();
                        return stream;
                    });
                    btnBorrar.IsVisible = true;
                }
            }
        }

        private async void btnCam_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Existe Camara", ":( No camera avaialble.", "OK");
                return;
            }

            var foto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                Name = "MiFoto.jpg",
                PhotoSize = PhotoSize.Small
            });

            if (foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    foto.GetStream().CopyTo(memoryStream);
                    VarGlobal.dat_evidencia = memoryStream.ToArray();
                    VarGlobal.tip_evidencia = "image/jpeg";
                }
                btnBorrar.IsVisible = true;
            }
            img_foto.Source = ImageSource.FromStream(() =>
            {
                var stream = foto.GetStream();
                VarGlobal.tam_evidencia = stream.Length;
                foto.Dispose();
                return stream;
            });

        }

        private void btnBorrar_Clicked(object sender, EventArgs e)
        {
            img_foto.Source = "";
            btnBorrar.IsVisible = false;
        }
        #endregion
    }
}
