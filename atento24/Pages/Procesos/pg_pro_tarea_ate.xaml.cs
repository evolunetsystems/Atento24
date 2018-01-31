using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using atento24.Data.DataLite;
using atento24.Data.ORM;
using atento24.Pages.Popup;
using atento24.Recursos;
using Plugin.Iconize;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages.Procesos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_pro_tarea_ate : ContentPage
    {
        public static List<lc_aux_estado> plst_estado = new List<lc_aux_estado>();
        public pg_pro_tarea_ate()
        {
            InitializeComponent();
            var comentar = VarGlobal.comentar;
            if (comentar == 1)
            {
                slAvance.IsVisible = false;
                slEstado.IsVisible = false;
            }
            else
            {
                var opcion = VarGlobal.ver_opcion;
                switch (opcion)
                {
                    case "A":
                        slAvance.IsVisible = true;
                        slEstado.IsVisible = false;
                        break;
                    case "V":
                        slAvance.IsVisible = false;
                        slEstado.IsVisible = true;
                        break;
                }
            }

            int porcentaje = Convert.ToInt32(VarGlobal.pro_tarea.por_avance);
            if (porcentaje == 0)
            {
                btnRestar.IsEnabled = false;
            }
            edDescripcion.Text = VarGlobal.pro_tarea.des_tarea;
            lblAvance.Text = porcentaje.ToString();
            CargarAvances();
        }

        private void CargarAvances()
        {
            lc_pro_avance_Data o_Data = new lc_pro_avance_Data();
            var lista = o_Data.Listar();
            List<lc_pro_avance> lst_avance = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                        && x.cod_unidad == VarGlobal.cod_unidad
                                                                        && x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).OrderByDescending(x => x.num_avance).ToList();

            CargarEvidencias();

            for (int i = 0; i < lst_avance.Count; i++)
            {
                int i_num_etapa = lst_avance[i].num_avance;
                var ent_evidencia = VarGlobal.pro_tarea.lst_lc_pro_evidencia.Where(x => x.num_etapa == i_num_etapa).FirstOrDefault();

                var tipo = lst_avance[i].tip_avance;
                if (tipo == "A")
                {
                    lst_avance[i].vpor_avance = lst_avance[i].por_avance.ToString().Trim() + "%";
                }
                else
                {
                    lst_avance[i].vpor_avance = "";
                }

                //  CREANDO STACKLAYOUT CONTENEDOR.
                StackLayout stCon = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 1),
                    Spacing = 0,
                    Padding = 5,
                    BackgroundColor = Color.White,
                    Children = {
                                new IconImage{
                                Icon="fa-user-circle",
                                IconColor = Color.Black,
                                VerticalOptions = LayoutOptions.Start,
                                Margin = new Thickness(0, 0, 5, 0),
                                IconSize = 20,
                                WidthRequest = 30,
                                HeightRequest = 30
                            }
                        }
                };

                StackLayout stHij = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Spacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                            new Label {
                                Text = lst_avance[i].nom_personal,
                                TextColor = Color.Black,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 12
                            },
                            new Label {
                                Text = lst_avance[i].fec_avance, Margin = 0,
                                FontSize = 11,
                                TextColor = Color.Gray
                            },
                            new Label {
                                Text = lst_avance[i].vpor_avance.ToString().Trim() + " de Avance.",
                                FontSize = 11,
                                TextColor = Color.FromHex("#5F6A6A"),
                                IsVisible = (lst_avance[i].tip_avance == "A")
                            },
                            new Label {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Text = lst_avance[i].des_avance.Trim(),
                                Margin = new Thickness(0, 5, 0, 0),
                                FontSize = 11,
                                TextColor = Color.FromHex("#2E2E2E")
                            }
                        }
                };

                // CREANDO IMAGEN
                if (ent_evidencia != null)
                {
                    Stream stream = new MemoryStream(ent_evidencia.dat_evidencia);
                    Image imgFoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return stream; }),
                        HeightRequest = 150,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    stHij.Children.Add(imgFoto);
                }
                stCon.Children.Add(stHij);
                stPri.Children.Add(stCon);
            }
            aiAva.IsVisible = false;

        }

        private void CargarEvidencias()
        {
            lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
            List<lc_pro_evidencia> lst_evidencia = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                        && x.cod_unidad == VarGlobal.cod_unidad
                                                                        && x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).ToList();
            VarGlobal.pro_tarea.lst_lc_pro_evidencia = lst_evidencia;
        }

        private async void pkEstadoLabel_Tapped(object sender, EventArgs e)
        {
            lc_aux_estado_Data o_Data = new lc_aux_estado_Data();
            var lst_estado = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                            && x.cod_modulo == "TR").ToList();
            //obetener etapa
            int i_etapa = lst_estado.Where(x => x.cod_estado == VarGlobal.pro_tarea.cod_estado).FirstOrDefault().eta_estado;
            plst_estado = lst_estado.Where(x => x.eta_estado == (i_etapa + 1)).ToList();

            List<ent_mensaje> lst_mensaje = new List<ent_mensaje>();
            VarGlobal.tit_mensaje = "ESTADOS";
            for (int i = 0; i < plst_estado.Count; i++)
            {
                lst_mensaje.Add(new ent_mensaje
                {
                    cod_mensaje = plst_estado[i].cod_estado,
                    ico_mensaje = "fa-check",
                    tex_mensaje = plst_estado[i].nom_estado
                });
            }

            var popupAlert = new pg_lista(lst_mensaje);
            var result = await popupAlert.Show();
            if (result != null)
            {
                VarGlobal.pro_tarea.cod_estado = result;
                VarGlobal.pro_tarea.nom_estado = plst_estado.Where(x => x.cod_estado == result).FirstOrDefault().nom_estado.ToString();

                pkEstadoLabel.Text = VarGlobal.pro_tarea.nom_estado;
            }
        }

        private async void btnGrabar_Clicked(object sender, EventArgs e)
        {
            lc_aux_estado_Data o_Data_Est = new lc_aux_estado_Data();
            var loadingPage = new pg_Loading();
            var comentar = VarGlobal.comentar;
            var imagen = img_foto.Source;

            if (comentar == 1)
            {
                if (!ValidarComentar())
                {
                    var popupAlert = new pg_confirmacion(new ent_mensaje
                    {
                        tip_mensaje = "INF",
                        tit_mensaje = "Tárea",
                        tex_mensaje = "¿Desea Comentar Tárea?"
                    });
                    var result = await popupAlert.Show();
                    await Navigation.PushPopupAsync(loadingPage);

                    if (result)
                    {
                        lc_pro_avance_Data o_Data = new lc_pro_avance_Data();
                        var i_numetapa = o_Data.Listar().Where(x => x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).Count();
                        VarGlobal.num_etapa = (i_numetapa + 1);


                        GrabarAvance("C");
                        lc_pro_tarea_Data o_Data_Tar = new lc_pro_tarea_Data();
                        VarGlobal.pro_tarea.por_avance = Convert.ToInt32(lblAvance.Text);
                        VarGlobal.pro_tarea.des_avance = edComentario.Text;
                        VarGlobal.pro_tarea.sincronizado = false;
                        GrabarEvidencia();
                        o_Data_Tar.Modificar(VarGlobal.pro_tarea);

                        Retornar();
                    }
                    await Navigation.RemovePopupPageAsync(loadingPage);
                }
            }
            else
            {
                var opcion = VarGlobal.ver_opcion;
                switch (opcion)
                {
                    case "A":
                        if (!ValidarAtender())
                        {
                            var popupAlert = new pg_confirmacion(new ent_mensaje
                            {
                                tip_mensaje = "INF",
                                tit_mensaje = "Tárea",
                                tex_mensaje = "¿Desea Atender Tárea?"
                            });
                            var result = await popupAlert.Show();
                            await Navigation.PushPopupAsync(loadingPage);

                            if (result)
                            {
                                lc_pro_avance_Data o_Data = new lc_pro_avance_Data();
                                var i_numetapa = o_Data.Listar().Where(x => x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).Count();
                                VarGlobal.num_etapa = (i_numetapa + 1);
                                GrabarAvance("A");

                                lc_pro_tarea_Data o_Data_Tar = new lc_pro_tarea_Data();
                                VarGlobal.pro_tarea.por_avance = Convert.ToInt32(lblAvance.Text);
                                VarGlobal.pro_tarea.des_avance = edComentario.Text;
                                VarGlobal.pro_tarea.sincronizado = false;
                                if (VarGlobal.pro_tarea.por_avance == 100)
                                {
                                    lc_aux_estado ent_estado = o_Data_Est.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa
                                                                                        && x.cod_modulo == "TR"
                                                                                        && x.cod_estado == "02").FirstOrDefault();
                                    VarGlobal.pro_tarea.cod_estado = ent_estado.cod_estado;
                                    VarGlobal.pro_tarea.nom_estado = ent_estado.nom_estado;
                                    VarGlobal.pro_tarea.ver_opcion = ent_estado.ver_opcion;
                                }
                                GrabarEvidencia();
                                o_Data_Tar.Modificar(VarGlobal.pro_tarea);

                                Retornar();
                            }
                            await Navigation.RemovePopupPageAsync(loadingPage);
                        }
                        break;
                    case "V":
                        if (!ValidarVerificar())
                        {
                            var popupAlert = new pg_confirmacion(new ent_mensaje
                            {
                                tip_mensaje = "INF",
                                tit_mensaje = "Tárea",
                                tex_mensaje = "¿Desea Verificar Tárea?"
                            });
                            var result = await popupAlert.Show();
                            await Navigation.PushPopupAsync(loadingPage);

                            if (result)
                            {
                                lc_pro_estado_Data o_Data = new lc_pro_estado_Data();
                                var i_numetapa = o_Data.Listar().Where(x => x.cod_referencia == VarGlobal.pro_tarea.cod_tarea).Count();
                                VarGlobal.num_etapa = (i_numetapa + 1);

                                //lc_aux_estado pk_estado = pkEstado.SelectedItem as lc_aux_estado;
                                lc_aux_estado pk_estado = plst_estado.Where(x => x.cod_estado == VarGlobal.pro_tarea.cod_estado).FirstOrDefault();

                                o_Data.Insertar(new lc_pro_estado
                                {
                                    cod_empresa = VarGlobal.cod_empresa,
                                    cod_unidad = VarGlobal.cod_unidad,
                                    cod_personal = VarGlobal.cod_personal,
                                    cod_estado = pk_estado.cod_estado,
                                    des_estado = edComentario.Text,
                                    cod_modulo = "TR",
                                    sincronizado = false,
                                    cod_referencia = VarGlobal.pro_tarea.cod_tarea,
                                    usuario = VarGlobal.cod_usuario,
                                    ip = "App"
                                });

                                lc_pro_tarea_Data o_Data_Tar = new lc_pro_tarea_Data();
                                VarGlobal.pro_tarea.des_avance = edComentario.Text;
                                VarGlobal.pro_tarea.sincronizado = false;
                                VarGlobal.pro_tarea.cod_estado = pk_estado.cod_estado;
                                VarGlobal.pro_tarea.nom_estado = pk_estado.nom_estado;
                                VarGlobal.pro_tarea.ver_opcion = pk_estado.ver_opcion;

                                GrabarEvidencia();
                                o_Data_Tar.Modificar(VarGlobal.pro_tarea);

                                Retornar();
                            }
                            await Navigation.RemovePopupPageAsync(loadingPage);
                        }
                        break;
                }
            }
        }

        private void GrabarAvance(string s_tipo_avance)
        {
            lc_pro_avance_Data o_Data = new lc_pro_avance_Data();

            o_Data.Insertar(new lc_pro_avance
            {
                cod_empresa = VarGlobal.pro_tarea.cod_empresa,
                cod_unidad = VarGlobal.pro_tarea.cod_unidad,
                cod_personal = VarGlobal.cod_personal,
                nom_personal = VarGlobal.nom_personal,
                fec_avance = DateTime.Now.ToString("dd/MM/yyyy"),
                por_avance = Convert.ToInt32(lblAvance.Text),
                des_avance = edComentario.Text,
                num_avance = VarGlobal.num_etapa,
                tip_avance = s_tipo_avance,
                cod_modulo = "TR",
                sincronizado = false,
                cod_referencia = VarGlobal.pro_tarea.cod_tarea,
                usuario = VarGlobal.cod_usuario,
                ip = VarGlobal.ip
            });
        }

        private void GrabarEvidencia()
        {
            var imagen = img_foto.Source;
            if (imagen != null)
            {
                lc_pro_evidencia ent_imagen = new lc_pro_evidencia()
                {
                    cod_empresa = VarGlobal.cod_empresa,
                    cod_unidad = VarGlobal.cod_unidad,
                    cod_referencia = VarGlobal.pro_tarea.cod_tarea,
                    cod_modulo = "TR",
                    dat_evidencia = VarGlobal.dat_evidencia,
                    nom_evidencia = "evidencia.jpg",
                    tip_evidencia = VarGlobal.tip_evidencia,
                    tam_evidencia = VarGlobal.tam_evidencia,
                    com_evidencia = "", // edComentario.Text,
                    tip_etapa = VarGlobal.tip_etapa,
                    num_etapa = VarGlobal.num_etapa,
                    sincronizado = false,
                    usuario = VarGlobal.cod_usuario,
                    ip = VarGlobal.ip,
                    estado = "A",
                    comando = "INS"
                };

                lc_pro_evidencia_Data o_Data = new lc_pro_evidencia_Data();
                o_Data.Insertar(ent_imagen);
            }
        }

        private bool ValidarAtender()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(edComentario.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Comentario...";
            }

            if (b_Error)
            {
                DisplayAlert("Validación", s_Mensaje, "Aceptar");
            }

            return b_Error;
        }

        private bool ValidarVerificar()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(edComentario.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Comentario...";
            }

            if (string.IsNullOrEmpty(pkEstadoLabel.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Estado...";
            }

            if (b_Error)
            {
                DisplayAlert("Validación", s_Mensaje, "Aceptar");
            }

            return b_Error;
        }

        private bool ValidarComentar()
        {
            bool b_Error = false;
            string s_Mensaje = "";

            if (string.IsNullOrEmpty(edComentario.Text))
            {
                b_Error = true;
                s_Mensaje = "Ingresar Comentario...";
            }

            if (b_Error)
            {
                DisplayAlert("Validación", s_Mensaje, "Aceptar");
            }

            return b_Error;
        }

        private void btnRestar_Clicked(object sender, EventArgs e)
        {
            decimal valor = Convert.ToDecimal(lblAvance.Text);
            decimal val_resta = 5;
            int val_final = Convert.ToInt32(valor - val_resta);

            if (val_final > 0)
            {
                lblAvance.Text = Convert.ToString(val_final);
                btnSumar.IsEnabled = true;
            }
            else
            {
                lblAvance.Text = Convert.ToString(0);
                btnRestar.IsEnabled = false;
            }
        }

        private void btnSumar_Clicked(object sender, EventArgs e)
        {
            decimal valor = Convert.ToDecimal(lblAvance.Text);
            decimal val_suma = 5;
            int val_final = Convert.ToInt32(valor + val_suma);

            if (val_final < 100)
            {
                lblAvance.Text = Convert.ToString(val_final);
                btnRestar.IsEnabled = true;
            }
            else
            {
                lblAvance.Text = Convert.ToString(100);
                btnSumar.IsEnabled = false;
            }
        }

        private void btn_salir_Clicked(object sender, EventArgs e)
        {
            Retornar();
        }

        private async void Retornar()
        {
            Content.IsEnabled = false;
            var loadingPage = new pg_Loading();
            await Navigation.PushPopupAsync(loadingPage);

            VarGlobal.pro_tarea.ver_opcion = "";
            string retorno = VarGlobal.ret_tarea_hijo;
            await Navigation.PushModalAsync(new MasterDetailPage1(retorno));

            await Navigation.RemovePopupPageAsync(loadingPage);
            Content.IsEnabled = true;
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
