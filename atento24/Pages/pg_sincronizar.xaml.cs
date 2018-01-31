using System;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;
using Newtonsoft.Json;
using atento24.Data.DataLite;
using atento24.Pages.Popup;
using atento24.Recursos;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using atento24.Data.ORM;

namespace atento24.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_sincronizar : ContentPage
    {
        public pg_Loading loadingPage = new pg_Loading();

        #region DATAS
        public lc_acc_empresa_Data lc_acc_empresa_Data = new lc_acc_empresa_Data();
        public lc_acc_unidad_Data lc_acc_unidad_Data = new lc_acc_unidad_Data();
        public lc_acc_menu_Data lc_acc_menu_Data = new lc_acc_menu_Data();
        public lc_acc_usuario_Data lc_acc_usuario_Data = new lc_acc_usuario_Data();
        public lc_cat_equipo_Data lc_cat_equipo_Data = new lc_cat_equipo_Data();
        public lc_cat_labor_Data lc_cat_labor_Data = new lc_cat_labor_Data();
        public lc_cat_lugar_Data lc_cat_lugar_Data = new lc_cat_lugar_Data();
        public lc_cat_ocurrencia_Data lc_cat_ocurrencia_Data = new lc_cat_ocurrencia_Data();
        public lc_cat_personal_Data lc_cat_personal_Data = new lc_cat_personal_Data();
        public lc_cat_inspeccionpre_Data lc_cat_inspeccionpre_Data = new lc_cat_inspeccionpre_Data();
        public lc_cat_veoplantilla_Data lc_cat_veoplantilla_Data = new lc_cat_veoplantilla_Data();
        public lc_cat_veoplantilla_lncontrol_Data lc_cat_veoplantilla_lncontrol_Data = new lc_cat_veoplantilla_lncontrol_Data();

        public lc_acc_unidad_tipoubicacion_Data lc_acc_unidad_tipoubicacion_Data = new lc_acc_unidad_tipoubicacion_Data();
        public lc_acc_unidad_sisgestion_Data lc_acc_unidad_sisgestion_Data = new lc_acc_unidad_sisgestion_Data();
        public lc_aux_tbldetalle_Data lc_aux_tbldetalle_Data = new lc_aux_tbldetalle_Data();
        public lc_glb_hallazgoclase_Data lc_glb_hallazgoclase_Data = new lc_glb_hallazgoclase_Data();
        public lc_glb_inspecciontipo_Data lc_glb_inspecciontipo_Data = new lc_glb_inspecciontipo_Data();
        public lc_aux_estado_Data lc_aux_estado_Data = new lc_aux_estado_Data();
        public lc_acc_unidad_severidadpot_Data lc_acc_unidad_severidadpot_Data = new lc_acc_unidad_severidadpot_Data();
        public lc_acc_unidad_severidadreal_Data lc_acc_unidad_severidadreal_Data = new lc_acc_unidad_severidadreal_Data();

        public lc_pro_hallazgo_Data lc_pro_hallazgo_Data = new lc_pro_hallazgo_Data();
        public lc_pro_coordenada_Data lc_pro_coordenada_Data = new lc_pro_coordenada_Data();
        public lc_pro_tarea_Data lc_pro_tarea_Data = new lc_pro_tarea_Data();
        public lc_pro_evidencia_Data lc_pro_evidencia_Data = new lc_pro_evidencia_Data();
        public lc_pro_participante_Data lc_pro_participante_Data = new lc_pro_participante_Data();
        public lc_pro_veoregistro_Data lc_pro_veoregistro_Data = new lc_pro_veoregistro_Data();
        public lc_pro_veoregistro_lncontrol_Data lc_pro_veoregistro_lncontrol_Data = new lc_pro_veoregistro_lncontrol_Data();
        public lc_pro_inspeccion_Data lc_pro_inspeccion_Data = new lc_pro_inspeccion_Data();
        public lc_pro_avance_Data lc_pro_avance_Data = new lc_pro_avance_Data();
        public lc_pro_incidente_Data lc_pro_incidente_Data = new lc_pro_incidente_Data();
        public lc_pro_incidente_personal_Data lc_pro_incidente_personal_Data = new lc_pro_incidente_personal_Data();
        public lc_pro_estado_Data lc_pro_estado_Data = new lc_pro_estado_Data();
        public lc_pro_elimina_Data lc_pro_elimina_Data = new lc_pro_elimina_Data();
        #endregion

        public static bool b_subir_cont = false;
        public double d_tabla = 24.0;
        public double d_conta = 24.0;
        public double d_tabla_subir = 5.0;
        public double d_conta_subir = 5.0;

        public pg_sincronizar()
        {
            InitializeComponent();
            IsBusy = false;
            ContarRegistros();
        }

        private void ContarRegistros()
        {
            int n_incidente = lc_pro_incidente_Data.Listar().Where(x => x.sincronizado == false).Count();
            int n_inspeccion = lc_pro_inspeccion_Data.Listar().Where(x => x.sincronizado == false).Count();
            int n_hallazgo = lc_pro_hallazgo_Data.Listar().Where(x => x.sincronizado == false).Count();
            int n_tarea = lc_pro_tarea_Data.Listar().Where(x => x.sincronizado == false).Count();
            int n_veo = lc_pro_veoregistro_Data.Listar().Where(x => x.sincronizado == false).Count();

            int total = n_incidente + n_inspeccion + n_hallazgo + n_tarea + n_veo;
            lb_numeric.Text = Convert.ToString(total).Trim();

            if (total > 0)
            {
                fm_Bajar.IsEnabled = false;
                fm_Subir.IsEnabled = true;

                //
                fm_Bajar.BackgroundColor = Color.FromHex("#A6ACAA");
                //btnBajar.BackgroundColor = Color.FromHex("#7D8381");

            }
            else
            {
                fm_Bajar.IsEnabled = true;
                fm_Subir.IsEnabled = false;

                //
                fm_Subir.BackgroundColor = Color.FromHex("#A6ACAA");
                //btnSubir.BackgroundColor = Color.FromHex("#7D8381");
            }
        }

        private async void btnSyn_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "INF",
                tit_mensaje = "Confirmar Sincronización",
                tex_mensaje = "¿Desea Sincronizar datos de la nube al dispositivo móvil?"
            });

            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)
            await Navigation.PushPopupAsync(loadingPage);
            if (result)
            {
                var usuario = lc_acc_usuario_Data.ListarUno();
                VarGlobal.cod_usuario = usuario.cod_usuario;
                EliminarRegistros();
                #region HILOS

                //no funcionan los hilos en ios
                Thread Hilo1 = new Thread(SincronizarEmpresa);
                Hilo1.Start();

                Thread Hilo2 = new Thread(SincronizarUnidad);
                Hilo2.Start();

                Thread Hilo3 = new Thread(SincronizarMenu);
                Hilo3.Start();

                Thread Hilo4 = new Thread(SincronizarEquipo);
                Hilo4.Start();

                Thread Hilo5 = new Thread(SincronizarLugar);
                Hilo5.Start();

                Thread Hilo6 = new Thread(SincronizarLabor);
                Hilo6.Start();

                Thread Hilo7 = new Thread(SincronizarOcurrencia);
                Hilo7.Start();

                Thread Hilo8 = new Thread(SincronizarPersonal);
                Hilo8.Start();

                Thread Hilo9 = new Thread(SincronizarVeoplantilla);
                Hilo9.Start();

                Thread Hilo10 = new Thread(SincronizarUbicacion);
                Hilo10.Start();

                Thread Hilo11 = new Thread(SincronizarSisGestion);
                Hilo11.Start();

                Thread Hilo12 = new Thread(SincronizarTblDetalle);
                Hilo12.Start();

                Thread Hilo13 = new Thread(SincronizarHallazgoCla);
                Hilo13.Start();

                Thread Hilo14 = new Thread(SincronizarProHallazgo);
                Hilo14.Start();

                Thread Hilo15 = new Thread(SincronizarProVeoRegistro);
                Hilo15.Start();

                Thread Hilo16 = new Thread(SincronizarVeoplantillaControl);
                Hilo16.Start();

                Thread Hilo17 = new Thread(SincronizarProInspeccion);
                Hilo17.Start();

                Thread Hilo18 = new Thread(SincronizarProTarea);
                Hilo18.Start();

                Thread Hilo19 = new Thread(SincronizarEstado);
                Hilo19.Start();

                Thread Hilo20 = new Thread(Sincronizarincidente);
                Hilo20.Start();

                Thread Hilo21 = new Thread(SincronizarSevReal);
                Hilo21.Start();

                Thread Hilo22 = new Thread(SincronizarSevPote);
                Hilo22.Start();

                Thread Hilo23 = new Thread(SincronizarInspeccionPre);
                Hilo23.Start();

                Thread Hilo24 = new Thread(SincronizarInspeccionTipo);
                Hilo24.Start();
                #endregion
            }
            else
            {
                await Navigation.RemovePopupPageAsync(loadingPage);
            }

        }

        private void EliminarRegistros()
        {
            lc_cat_veoplantilla_Data.Eliminar();
            lc_cat_veoplantilla_lncontrol_Data.Eliminar();
            lc_cat_personal_Data.Eliminar();
            lc_cat_ocurrencia_Data.Eliminar();
            lc_cat_labor_Data.Eliminar();
            lc_cat_lugar_Data.Eliminar();
            lc_cat_equipo_Data.Eliminar();
            lc_acc_menu_Data.Eliminar();
            lc_acc_unidad_Data.Eliminar();
            lc_acc_empresa_Data.Eliminar();
            lc_acc_unidad_tipoubicacion_Data.Eliminar();
            lc_acc_unidad_sisgestion_Data.Eliminar();
            lc_aux_tbldetalle_Data.Eliminar();
            lc_aux_estado_Data.Eliminar();
            lc_glb_hallazgoclase_Data.Eliminar();
            lc_glb_inspecciontipo_Data.Eliminar();
            lc_cat_inspeccionpre_Data.Eliminar();

            lc_pro_hallazgo_Data.Eliminar();
            lc_pro_tarea_Data.Eliminar();
            lc_pro_evidencia_Data.Eliminar();
            lc_pro_coordenada_Data.Eliminar();
            lc_pro_participante_Data.Eliminar();
            lc_pro_veoregistro_Data.Eliminar();
            //lc_pro_veoregistro_lncontrol_Data.Eliminar();
            lc_pro_inspeccion_Data.Eliminar();
            lc_pro_avance_Data.Eliminar();
            lc_pro_estado_Data.Eliminar();
            lc_pro_incidente_Data.Eliminar();
            //lc_pro_incidente_personal_Data.Eliminar();
            lc_acc_unidad_severidadpot_Data.Eliminar();
            lc_acc_unidad_severidadreal_Data.Eliminar();
            lc_pro_elimina_Data.Eliminar();
        }

        private async void Contador()
        {
            Thread.Sleep(500);
            d_conta = --d_conta;
            lblNum.Text = "Restan " + d_conta.ToString().Trim() + " Tablas.";

            double d_por = 1 - Convert.ToDouble(d_conta / d_tabla);
            pgBarra.Progress = System.Math.Round(d_por, 1);

            if (d_conta == 0)
            {
                lc_acc_usuario_Data o_Data = new lc_acc_usuario_Data();
                lc_acc_usuario o_entidad = o_Data.ListarUno();
                o_entidad.syn_auxiliar = true;
                o_Data.Actualizar(o_entidad);
                await Navigation.PushModalAsync(new MasterDetailPage1("pg_empresa"));
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        private async void Contador_subir()
        {
            Thread.Sleep(500);
            d_conta_subir = --d_conta_subir;
            lblNum.Text = "Restan " + d_conta_subir.ToString().Trim() + " Tablas.";

            double d_por = 1 - Convert.ToDouble(d_conta_subir / d_tabla_subir);
            pgBarra.Progress = System.Math.Round(d_por, 1);

            if (d_conta_subir == 0)
            {
                lc_acc_usuario_Data o_Data = new lc_acc_usuario_Data();
                lc_acc_usuario o_entidad = o_Data.ListarUno();
                o_entidad.syn_auxiliar = true;
                o_Data.Actualizar(o_entidad);
                await Navigation.PushModalAsync(new MasterDetailPage1("pg_empresa"));
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }

        #region Catalogos
        private void SincronizarInspeccionPre()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_inspeccionpre"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_inspeccionpre> lista = JsonConvert.DeserializeObject<List<lc_cat_inspeccionpre>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_inspeccionpre_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarVeoplantilla()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_veoplantilla"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_veoplantilla> lista = JsonConvert.DeserializeObject<List<lc_cat_veoplantilla>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_veoplantilla_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarVeoplantillaControl()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_veoplantilla_lncontrol"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_veoplantilla_lncontrol> lista = JsonConvert.DeserializeObject<List<lc_cat_veoplantilla_lncontrol>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_veoplantilla_lncontrol_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarPersonal()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_personal"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_personal> lista = JsonConvert.DeserializeObject<List<lc_cat_personal>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_personal_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarOcurrencia()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_ocurrencia"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_ocurrencia> lista = JsonConvert.DeserializeObject<List<lc_cat_ocurrencia>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_ocurrencia_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarLabor()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_labor"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_labor> lista = JsonConvert.DeserializeObject<List<lc_cat_labor>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_labor_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarLugar()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_lugar"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_lugar> lista = JsonConvert.DeserializeObject<List<lc_cat_lugar>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_lugar_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarEquipo()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "cat_equipo"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_cat_equipo> lista = JsonConvert.DeserializeObject<List<lc_cat_equipo>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_cat_equipo_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }
        #endregion

        #region Accesos y Auxiliares

        private void SincronizarMenu()
        {
            var responseMenu = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_menu"
            });

            if (responseMenu.IsSuccessStatusCode)
            {
                List<lc_acc_menu> lstMenu = JsonConvert.DeserializeObject<List<lc_acc_menu>>(responseMenu.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lstMenu.Count; i++)
                    {
                        lc_acc_menu_Data.Insertar(lstMenu[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarEmpresa()
        {
            var responseEmpresa = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_empresa"
            });

            if (responseEmpresa.IsSuccessStatusCode)
            {
                List<lc_acc_empresa> lstEmpresa = JsonConvert.DeserializeObject<List<lc_acc_empresa>>(responseEmpresa.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lstEmpresa.Count; i++)
                    {
                        lc_acc_empresa_Data.Insertar(lstEmpresa[i]);
                    }
                    Contador();
                });
            }

        }

        private void SincronizarUnidad()
        {
            var responseUnidad = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_unidad"
            });

            if (responseUnidad.IsSuccessStatusCode)
            {
                List<lc_acc_unidad> lstUnidad = JsonConvert.DeserializeObject<List<lc_acc_unidad>>(responseUnidad.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lstUnidad.Count; i++)
                    {
                        lc_acc_unidad_Data.Insertar(lstUnidad[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarUbicacion()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_unidad_tipoubicacion"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_acc_unidad_tipoubicacion> lista = JsonConvert.DeserializeObject<List<lc_acc_unidad_tipoubicacion>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_acc_unidad_tipoubicacion_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarHallazgoCla()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "glb_hallazgoclase"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_glb_hallazgoclase> lista = JsonConvert.DeserializeObject<List<lc_glb_hallazgoclase>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_glb_hallazgoclase_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarTblDetalle()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "aux_tbldetalle"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_aux_tbldetalle> lista = JsonConvert.DeserializeObject<List<lc_aux_tbldetalle>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_aux_tbldetalle_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarEstado()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "aux_estado"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_aux_estado> lista = JsonConvert.DeserializeObject<List<lc_aux_estado>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_aux_estado_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarSisGestion()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_unidad_sisgestion"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_acc_unidad_sisgestion> lista = JsonConvert.DeserializeObject<List<lc_acc_unidad_sisgestion>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_acc_unidad_sisgestion_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarRiesgo()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "aux_tbldetalle"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_aux_tbldetalle> lista = JsonConvert.DeserializeObject<List<lc_aux_tbldetalle>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_aux_tbldetalle_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarSevPote()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_unidad_severidadpot"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_acc_unidad_severidadpot> lista = JsonConvert.DeserializeObject<List<lc_acc_unidad_severidadpot>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_acc_unidad_severidadpot_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarSevReal()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "acc_unidad_severidadreal"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_acc_unidad_severidadreal> lista = JsonConvert.DeserializeObject<List<lc_acc_unidad_severidadreal>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_acc_unidad_severidadreal_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }

        private void SincronizarInspeccionTipo()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "glb_inspecciontipo"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_glb_inspecciontipo> lista = JsonConvert.DeserializeObject<List<lc_glb_inspecciontipo>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        lc_glb_inspecciontipo_Data.Insertar(lista[i]);
                    }
                    Contador();
                });
            }
        }
        #endregion

        #region Procesos
        private void SincronizarProHallazgo()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "pro_hallazgo"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_pro_hallazgo> lista = JsonConvert.DeserializeObject<List<lc_pro_hallazgo>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    InsertarProHallazgo(lista);
                    if (b_subir_cont)
                    {
                        Contador_subir();
                    }
                    else
                    {
                        Contador();
                    }

                });
            }
        }

        private void InsertarProHallazgo(List<lc_pro_hallazgo> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                //  Verificar Existencia.
                int iReg = lc_pro_hallazgo_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                              && x.cod_unidad == lista[i].cod_unidad
                                              && x.cod_hallazgo == lista[i].cod_hallazgo).Count();
                if (iReg == 0)
                {
                    //Insertar Evidencia
                    var lst_evidencia = lista[i].lst_lc_pro_evidencia;
                    for (int e = 0; e < lst_evidencia.Count; e++)
                    {
                        lc_pro_evidencia_Data.Insertar(lst_evidencia[e]);
                    }

                    //Insertar Coordenada
                    var lst_coordenada = lista[i].lst_lc_pro_coordenada;
                    for (int c = 0; c < lst_coordenada.Count; c++)
                    {
                        lc_pro_coordenada_Data.Insertar(lst_coordenada[c]);
                    }

                    //Insertar Participante
                    var lst_participante = lista[i].lst_lc_pro_participante;
                    for (int p = 0; p < lst_participante.Count; p++)
                    {
                        lc_pro_participante_Data.Insertar(lst_participante[p]);
                    }

                    //Insertar Tareas
                    var lst_tarea = lista[i].lst_lc_pro_tarea;
                    InsertarProTarea(lst_tarea);

                    lc_pro_hallazgo_Data.InsertarSinc(lista[i]);
                }

            }
        }

        private void InsertarProTarea(List<lc_pro_tarea> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                //  Verificar Existencia.
                int iReg = lc_pro_tarea_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                              && x.cod_unidad == lista[i].cod_unidad
                                              && x.cod_tarea == lista[i].cod_tarea).Count();
                if (iReg == 0)
                {
                    //Evidencia
                    var lst_evidencia = lista[i].lst_lc_pro_evidencia;
                    for (int e = 0; e < lst_evidencia.Count; e++)
                    {
                        lc_pro_evidencia_Data.Insertar(lst_evidencia[e]);
                    }

                    //Avance
                    var lst_avance = lista[i].lst_lc_pro_avance;
                    for (int a = 0; a < lst_avance.Count; a++)
                    {
                        lc_pro_avance_Data.Insertar(lst_avance[a]);
                    }

                    //Estado
                    var lst_estado = lista[i].lst_lc_pro_estado;
                    for (int x = 0; x < lst_estado.Count; x++)
                    {
                        lc_pro_estado_Data.Insertar(lst_estado[x]);
                    }

                    lc_pro_tarea_Data.Insertar(lista[i]);
                }
            }
        }

        private void SincronizarProVeoRegistro()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "pro_veoregistro"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_pro_veoregistro> lista = JsonConvert.DeserializeObject<List<lc_pro_veoregistro>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        //  Verificar Existencia.
                        int iReg = lc_pro_veoregistro_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                      && x.cod_unidad == lista[i].cod_unidad
                                                      && x.cod_veoregistro == lista[i].cod_veoregistro).Count();
                        if (iReg == 0)
                        {
                            //Insertar Participante
                            var lst_participante = lista[i].lst_lc_pro_participante;
                            for (int p = 0; p < lst_participante.Count; p++)
                            {
                                lc_pro_participante_Data.Insertar(lst_participante[p]);
                            }

                            //Insertar Coordenada
                            var lst_coordenada = lista[i].lst_lc_pro_coordenada;
                            for (int c = 0; c < lst_coordenada.Count; c++)
                            {
                                lc_pro_coordenada_Data.Insertar(lst_coordenada[c]);
                            }

                            //Insertar Lineas de control
                            var lst_lncontrol = lista[i].lst_lc_pro_veoregistro_lncontrol;
                            for (int l = 0; l < lst_lncontrol.Count; l++)
                            {
                                lc_pro_veoregistro_lncontrol_Data.Insertar(lst_lncontrol[l]);
                            }
                        }

                        lc_pro_veoregistro_Data.InsertarSinc(lista[i]);
                    }
                    if (b_subir_cont)
                    {
                        Contador_subir();
                    }
                    else
                    {
                        Contador();
                    }
                });
            }
        }

        private void SincronizarProInspeccion()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "pro_inspeccion"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_pro_inspeccion> lista = JsonConvert.DeserializeObject<List<lc_pro_inspeccion>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        //  Verificar Existencia.
                        int iReg = lc_pro_inspeccion_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                      && x.cod_unidad == lista[i].cod_unidad
                                                      && x.cod_inspeccion == lista[i].cod_inspeccion).Count();
                        if (iReg == 0)
                        {
                            //Insertar Hallazgo
                            var lst_hallazgo = lista[i].lst_lc_pro_hallazgo;
                            InsertarProHallazgo(lst_hallazgo);

                            //Insertar Participante
                            var lst_participante = lista[i].lst_lc_pro_participante;
                            for (int p = 0; p < lst_participante.Count; p++)
                            {
                                lc_pro_participante_Data.Insertar(lst_participante[p]);
                            }
                        }
                        lc_pro_inspeccion_Data.InsertarSinc(lista[i]);
                    }
                    if (b_subir_cont)
                    {
                        Contador_subir();
                    }
                    else
                    {
                        Contador();
                    }
                });
            }
        }

        private void SincronizarProTarea()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "pro_tarea"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_pro_tarea> lista = JsonConvert.DeserializeObject<List<lc_pro_tarea>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    InsertarProTarea(lista);

                    if (b_subir_cont)
                    {
                        Contador_subir();
                    }
                    else
                    {
                        Contador();
                    }
                });
            }
        }

        private void Sincronizarincidente()
        {
            var response = VarGlobal.LoadWebApiPost("pro_sincronizar", new lc_acc_usuario()
            {
                cod_usuario = VarGlobal.cod_usuario,
                tip_tabla = "pro_incidente"
            });

            if (response.IsSuccessStatusCode)
            {
                List<lc_pro_incidente> lista = JsonConvert.DeserializeObject<List<lc_pro_incidente>>(response.Content.ReadAsStringAsync().Result);
                Device.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        //  Verificar Existencia.
                        int iReg = lc_pro_incidente_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                      && x.cod_unidad == lista[i].cod_unidad
                                                      && x.cod_incidente == lista[i].cod_incidente).Count();
                        if (iReg == 0)
                        {
                            var lst_evidencia = lista[i].lst_lc_pro_evidencia;
                            for (int e = 0; e < lst_evidencia.Count; e++)
                            {
                                lc_pro_evidencia_Data.Insertar(lst_evidencia[e]);
                            }

                            var lst_personal = lista[i].lst_lc_pro_incidente_personal;
                            for (int p = 0; p < lst_personal.Count; p++)
                            {
                                lc_pro_incidente_personal_Data.Insertar(lst_personal[p]);
                            }

                            //Insertar Tareas
                            var lst_tarea = lista[i].lst_lc_pro_tarea;
                            InsertarProTarea(lst_tarea);

                            lc_pro_incidente_Data.InsertarSinc(lista[i]);
                        }
                    }
                    if (b_subir_cont)
                    {
                        Contador_subir();
                    }
                    else
                    {
                        Contador();
                    }
                });
            }
        }
        #endregion


        private async void Redireccionar()
        {
            await Navigation.PushModalAsync(new MasterDetailPage1("pg_empresa"));
        }

        private async void btnSubir_Clicked(object sender, EventArgs e)
        {
            var popupAlert = new pg_confirmacion(new ent_mensaje
            {
                tip_mensaje = "INF",
                tit_mensaje = "Confirmar Sincronización",
                tex_mensaje = "¿Desea Sincronizar datos del dispositivo móvil con la nube?"
            });

            var result = await popupAlert.Show(); //espere hasta que el usuario seleccione la opción (si o no)

            if (result)
            {
                await Navigation.PushPopupAsync(loadingPage);
                //  Sincronizar VEO
                SubirProVeoregistro();

                //  Sincronizar Incidente
                SubirProIncidente();

                //  Sincronizar Inpeccion, Hallazgos y Tareas
                SubirProInspeccion();

                //  Sincronizar Observaciones y Tareas
                SubirProHallazgo("", "", "OB");

                //  Sincronizar Tareas
                //SubirProTarea("", "", "TR");
                SubirTareasModificadas();

                BajarProcesos();
                //await Navigation.PushModalAsync(new MasterDetailPage1("pg_empresa"));
            }
            else
            {
                await Navigation.RemovePopupPageAsync(loadingPage);
            }

        }

        private void BajarProcesos()
        {
            lc_pro_hallazgo_Data.Eliminar();
            lc_pro_tarea_Data.Eliminar();
            lc_pro_evidencia_Data.Eliminar();
            lc_pro_coordenada_Data.Eliminar();
            lc_pro_participante_Data.Eliminar();
            lc_pro_veoregistro_Data.Eliminar();
            lc_pro_inspeccion_Data.Eliminar();
            lc_pro_incidente_Data.Eliminar();

            b_subir_cont = true;

            Thread Hilo1 = new Thread(SincronizarProHallazgo);
            Hilo1.Start();

            Thread Hilo2 = new Thread(SincronizarProVeoRegistro);
            Hilo2.Start();

            Thread Hilo3 = new Thread(SincronizarProInspeccion);
            Hilo3.Start();

            Thread Hilo4 = new Thread(SincronizarProTarea);
            Hilo4.Start();

            Thread Hilo5 = new Thread(Sincronizarincidente);
            Hilo5.Start();
        }

        private void SubirProIncidente()
        {
            lc_pro_incidente o_entidad = new lc_pro_incidente();
            List<lc_pro_incidente> lista = lc_pro_incidente_Data.Listar().Where(x => x.sincronizado == false).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                string s_codigo = lista[i].cod_incidente;

                o_entidad.cod_empresa = lista[i].cod_empresa;
                o_entidad.cod_unidad = lista[i].cod_unidad;
                o_entidad.cod_incidente = s_codigo;
                o_entidad.fec_incidente = lista[i].fec_incidente.Substring(0, 19);
                o_entidad.cod_sisgestion = lista[i].cod_sisgestion;
                o_entidad.cod_severidadreal = lista[i].cod_severidadreal;
                o_entidad.cod_severidadpot = lista[i].cod_severidadpot;
                o_entidad.des_incidente = lista[i].des_incidente;
                o_entidad.cod_tipoubicacion = lista[i].cod_tipoubicacion;
                o_entidad.cod_labor = lista[i].cod_labor;
                o_entidad.cod_lugar = lista[i].cod_lugar;
                o_entidad.cod_equipo = lista[i].cod_equipo;
                o_entidad.cod_personal = lista[i].cod_personal;
                o_entidad.cod_periodo = lista[i].cod_periodo;
                o_entidad.cod_estado = lista[i].cod_estado;
                o_entidad.por_avance = lista[i].por_avance;
                o_entidad.usuario = VarGlobal.cod_usuario;
                o_entidad.ip = VarGlobal.ip;
                o_entidad.estado = "A";
                o_entidad.comando = "INS";

                //Personal
                List<lc_pro_incidente_personal> lst_personal = lc_pro_incidente_personal_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_incidente == s_codigo
                                                       && x.sincronizado == false).ToList();
                o_entidad.lst_lc_pro_incidente_personal = lst_personal;

                //Evidencias
                List<lc_pro_evidencia> lst_evidencia = lc_pro_evidencia_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.cod_modulo == "IN").ToList();
                o_entidad.lst_lc_pro_evidencia = lst_evidencia;

                if (s_codigo.Length != 12) { o_entidad.cod_incidente = ""; }

                var response = VarGlobal.LoadWebApiPost("pro_incidente", o_entidad);
                lc_pro_incidente ent_incidente = JsonConvert.DeserializeObject<lc_pro_incidente>(response.Content.ReadAsStringAsync().Result);
                if (!response.IsSuccessStatusCode)
                {
                    DisplayAlert("Error", "Operación no realizada...", "Aceptar");
                }
                else
                {
                    //Actualizando campo de sincronizado
                    lista[i].sincronizado = true;
                    lc_pro_incidente_Data.Modificar(lista[i]);

                    SubirTareasNuevas(ent_incidente.cod_incidente, s_codigo, "IN");
                }
                //Contador();
            }
        }

        /// <summary>
        /// Metodo para Subir a la nube los Inspección
        /// </summary>
        private async void SubirProInspeccion()
        {
            lc_pro_inspeccion o_entidad = new lc_pro_inspeccion();
            List<lc_pro_inspeccion> lista = lc_pro_inspeccion_Data.Listar().Where(x => x.sincronizado == false).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                string s_codigo = lista[i].cod_inspeccion;

                o_entidad.cod_empresa = lista[i].cod_empresa;
                o_entidad.cod_unidad = lista[i].cod_unidad;
                o_entidad.cod_inspeccion = s_codigo;
                o_entidad.fec_inspeccion = lista[i].fec_inspeccion + " 00:00:00";
                o_entidad.cod_inspecciontipo = lista[i].cod_inspecciontipo;
                o_entidad.cod_inspeccionpre = lista[i].cod_inspeccionpre;
                o_entidad.tit_inspeccion = lista[i].tit_inspeccion;
                o_entidad.obj_inspeccion = lista[i].obj_inspeccion;
                o_entidad.cod_personal = lista[i].cod_personal;
                o_entidad.cod_estado = "01";
                o_entidad.cod_inspeccionprog = lista[i].cod_inspeccionprog;
                o_entidad.cod_sisgestion = lista[i].cod_sisgestion;
                o_entidad.usuario = VarGlobal.cod_usuario;
                o_entidad.ip = VarGlobal.ip;
                o_entidad.estado = "A";
                o_entidad.comando = "ACT";

                //Participantes
                List<lc_pro_participante> lst_participante = lc_pro_participante_Data.Listar().Where(x => x.cod_referencia == s_codigo).ToList();
                o_entidad.lst_lc_pro_participante = lst_participante;

                if (s_codigo.Length != 12) { o_entidad.cod_inspeccion = ""; }

                var response = VarGlobal.LoadWebApiPost("pro_inspeccion", o_entidad);
                lc_pro_inspeccion ent_inspeccion = JsonConvert.DeserializeObject<lc_pro_inspeccion>(response.Content.ReadAsStringAsync().Result);

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "Operación no realizada...", "Aceptar");
                }
                else
                {
                    //Actualizando campo de sincronizado
                    lista[i].sincronizado = true;
                    lc_pro_inspeccion_Data.Modificar(lista[i]);

                    //Sincronizando lista de hallazgos de inspecciones
                    SubirProHallazgo(ent_inspeccion.cod_inspeccion, s_codigo, "IP");
                }
            }
        }

        /// <summary>
        /// Metodo para Subir a la nube los hallazgos
        /// </summary>
        /// <param name="s_cod_generado"> Código generado en la nube para el origen (cod_referencia)</param>
        /// <param name="s_cod_original"> Código generado localmente para el origen (cod_referencia)</param>
        /// <param name="s_cod_modulo"> filtro por modulo</param>
        private void SubirProHallazgo(string s_cod_generado, string s_cod_original, string s_cod_modulo)
        {
            lc_pro_hallazgo o_entidad = new lc_pro_hallazgo();
            List<lc_pro_hallazgo> lista = lc_pro_hallazgo_Data.Listar().Where(x => x.sincronizado == false
                                                                        && x.cod_referencia.Trim() == s_cod_original
                                                                        && x.cod_modulo == s_cod_modulo).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                string s_codigo = lista[i].cod_hallazgo;

                o_entidad.cod_empresa = lista[i].cod_empresa;
                o_entidad.cod_unidad = lista[i].cod_unidad;
                o_entidad.cod_hallazgo = s_codigo;
                o_entidad.fec_hallazgo = lista[i].fec_hallazgo.Substring(0, 19);
                o_entidad.cod_hallazgoclase = lista[i].cod_hallazgoclase;
                o_entidad.cod_tblnivelriesgo = lista[i].cod_tblnivelriesgo;
                o_entidad.cod_periodo = lista[i].cod_periodo;
                o_entidad.cod_estado = lista[i].cod_estado;
                o_entidad.cod_ocurrencia = lista[i].cod_ocurrencia;
                o_entidad.cod_tblocurrenciatipo = lista[i].cod_tblocurrenciatipo;
                o_entidad.des_hallazgo = lista[i].des_hallazgo;
                o_entidad.cod_sisgestion = lista[i].cod_sisgestion;
                o_entidad.cod_tipoubicacion = lista[i].cod_tipoubicacion;
                o_entidad.cod_labor = lista[i].cod_labor;
                o_entidad.cod_lugar = lista[i].cod_lugar;
                o_entidad.cod_equipo = lista[i].cod_equipo;
                o_entidad.cod_personal = lista[i].cod_personal;
                o_entidad.cod_modulo = lista[i].cod_modulo;
                o_entidad.cod_referencia = s_cod_generado;
                o_entidad.por_avance = lista[i].por_avance;
                o_entidad.usuario = VarGlobal.cod_usuario;
                o_entidad.ip = VarGlobal.ip;
                o_entidad.estado = "A";
                o_entidad.comando = "INS";

                //Participantes
                List<lc_pro_participante> lst_participante = lc_pro_participante_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo).ToList();
                o_entidad.lst_lc_pro_participante = lst_participante;

                //Coordenadas
                List<lc_pro_coordenada> lst_coordenada = lc_pro_coordenada_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo).ToList();
                o_entidad.lst_lc_pro_coordenada = lst_coordenada;

                //Evidencias
                List<lc_pro_evidencia> lst_evidencia = lc_pro_evidencia_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo).ToList();
                o_entidad.lst_lc_pro_evidencia = lst_evidencia;

                if (s_codigo.Length != 12) { o_entidad.cod_hallazgo = ""; }

                var response = VarGlobal.LoadWebApiPost("pro_hallazgo", o_entidad);
                lc_pro_hallazgo ent_hallazgo = JsonConvert.DeserializeObject<lc_pro_hallazgo>(response.Content.ReadAsStringAsync().Result);
                if (!response.IsSuccessStatusCode)
                {
                    DisplayAlert("Error", "Operación no realizada...", "Aceptar");
                }
                else
                {
                    //Actualizando campo de sincronizado
                    //lista[i].sincronizado = true;
                    //lc_pro_hallazgo_Data.Modificar(lista[i]);

                    SubirTareasNuevas(ent_hallazgo.cod_hallazgo, s_codigo, "HL");
                }
                //Contador();
            }
        }

        /// <summary>
        /// Metodo para Subir a la nube las Tareas
        /// </summary>
        /// <param name="s_cod_generado"> Código generado en la nube para el origen (cod_referencia)</param>
        /// <param name="s_cod_original"> Código generado localmente para el origen (cod_referencia)</param>
        /// <param name="s_cod_modulo"> filtro por modulo</param>
        private void SubirTareasNuevas(string s_cod_generado, string s_cod_original, string s_cod_modulo)
        {
            lc_pro_tarea o_entidad = new lc_pro_tarea();
            //var lst_tarea = lc_pro_tarea_Data.Listar().Where(x => x.sincronizado == false ).ToList();
            List<lc_pro_tarea> lista = lc_pro_tarea_Data.Listar().Where(x => x.sincronizado == false
                                                    && x.cod_referencia.Trim() == s_cod_original
                                                    && x.cod_modulo == s_cod_modulo
                                                    && x.cod_tarea.Length != 12).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                string s_codigo = lista[i].cod_tarea;

                o_entidad.cod_empresa = lista[i].cod_empresa;
                o_entidad.cod_unidad = lista[i].cod_unidad;
                o_entidad.cod_tarea = s_codigo;
                o_entidad.ini_tarea = lista[i].ini_tarea;
                o_entidad.fin_tarea = lista[i].fin_tarea;
                o_entidad.cod_periodo = lista[i].cod_periodo;
                o_entidad.sol_personal = lista[i].sol_personal;
                o_entidad.eje_personal = lista[i].eje_personal;
                o_entidad.des_tarea = lista[i].des_tarea;
                o_entidad.cod_modulo = lista[i].cod_modulo;
                o_entidad.cod_modulo_2do = lista[i].cod_modulo_2do;
                o_entidad.cod_referencia = s_cod_generado;
                o_entidad.cod_tipoubicacion = lista[i].cod_tipoubicacion;
                o_entidad.cod_labor = lista[i].cod_labor;
                o_entidad.cod_lugar = lista[i].cod_lugar;
                o_entidad.cod_equipo = lista[i].cod_equipo;
                o_entidad.por_avance = lista[i].por_avance;
                o_entidad.des_avance = lista[i].des_avance;
                o_entidad.cod_estado = "01";
                o_entidad.usuario = VarGlobal.cod_usuario;
                o_entidad.ip = VarGlobal.ip;
                o_entidad.estado = "A";
                o_entidad.comando = "ACT";

                //Evidencias
                List<lc_pro_evidencia> lst_evidencia = lc_pro_evidencia_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.sincronizado == false
                                                       && x.cod_modulo == "TR").ToList();
                o_entidad.lst_lc_pro_evidencia = lst_evidencia;

                //Avance
                List<lc_pro_avance> lst_avance = lc_pro_avance_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.sincronizado == false
                                                       && x.cod_modulo == "TR").ToList();
                o_entidad.lst_lc_pro_avance = lst_avance;

                //Estado
                List<lc_pro_estado> lst_estado = lc_pro_estado_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.sincronizado == false
                                                       && x.cod_modulo == "TR").ToList();
                o_entidad.lst_lc_pro_estado = lst_estado;

                if (s_codigo.Length != 12) { o_entidad.cod_tarea = ""; }

                var response = VarGlobal.LoadWebApiPost("pro_tarea", o_entidad);
                lc_pro_tarea ent_hallazgo = JsonConvert.DeserializeObject<lc_pro_tarea>(response.Content.ReadAsStringAsync().Result);
                if (!response.IsSuccessStatusCode)
                {
                    DisplayAlert("Error", "Operación no realizada...", "Aceptar");
                }
            }
        }

        private void SubirTareasModificadas()
        {
            lc_pro_tarea o_entidad = new lc_pro_tarea();
            //var lst_tarea = lc_pro_tarea_Data.Listar().Where(x => x.sincronizado == false ).ToList();
            List<lc_pro_tarea> lista = lc_pro_tarea_Data.Listar().Where(x => x.sincronizado == false
                                                    && x.cod_tarea.Length == 12).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                string s_codigo = lista[i].cod_tarea;

                o_entidad.cod_empresa = lista[i].cod_empresa;
                o_entidad.cod_unidad = lista[i].cod_unidad;
                o_entidad.cod_tarea = s_codigo;
                o_entidad.ini_tarea = lista[i].ini_tarea;
                o_entidad.fin_tarea = lista[i].fin_tarea;
                o_entidad.cod_periodo = lista[i].cod_periodo;
                o_entidad.sol_personal = lista[i].sol_personal;
                o_entidad.eje_personal = lista[i].eje_personal;
                o_entidad.des_tarea = lista[i].des_tarea;
                o_entidad.cod_modulo = lista[i].cod_modulo;
                o_entidad.cod_modulo_2do = lista[i].cod_modulo_2do;
                o_entidad.cod_referencia = lista[i].cod_referencia;
                o_entidad.cod_tipoubicacion = lista[i].cod_tipoubicacion;
                o_entidad.cod_labor = lista[i].cod_labor;
                o_entidad.cod_lugar = lista[i].cod_lugar;
                o_entidad.cod_equipo = lista[i].cod_equipo;
                o_entidad.por_avance = lista[i].por_avance;
                o_entidad.des_avance = lista[i].des_avance;
                o_entidad.cod_estado = lista[i].cod_estado;
                o_entidad.usuario = VarGlobal.cod_usuario;
                o_entidad.ip = VarGlobal.ip;
                o_entidad.estado = "A";
                o_entidad.comando = "ACT";

                //Evidencias
                List<lc_pro_evidencia> lst_evidencia = lc_pro_evidencia_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.sincronizado == false
                                                       && x.cod_modulo == "TR").ToList();
                o_entidad.lst_lc_pro_evidencia = lst_evidencia;

                //Avance
                List<lc_pro_avance> lst_avance = lc_pro_avance_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.sincronizado == false
                                                       && x.cod_modulo == "TR").ToList();
                o_entidad.lst_lc_pro_avance = lst_avance;

                //Estado
                List<lc_pro_estado> lst_estado = lc_pro_estado_Data.Listar().Where(x => x.cod_empresa == lista[i].cod_empresa
                                                       && x.cod_unidad == lista[i].cod_unidad
                                                       && x.cod_referencia == s_codigo
                                                       && x.sincronizado == false
                                                       && x.cod_modulo == "TR").ToList();
                o_entidad.lst_lc_pro_estado = lst_estado;

                var response = VarGlobal.LoadWebApiPost("pro_tarea", o_entidad);
                lc_pro_tarea ent_hallazgo = JsonConvert.DeserializeObject<lc_pro_tarea>(response.Content.ReadAsStringAsync().Result);
                if (!response.IsSuccessStatusCode)
                {
                    DisplayAlert("Error", "Operación no realizada...", "Aceptar");
                }
            }
        }

        /// <summary>
        /// Metodo para Subir a la nube los Inspección
        /// </summary>
        private async void SubirProVeoregistro()
        {
            lc_pro_veoregistro o_entidad = new lc_pro_veoregistro();
            List<lc_pro_veoregistro> lista = lc_pro_veoregistro_Data.Listar().Where(x => x.sincronizado == false).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                string s_codigo = lista[i].cod_veoregistro;
                o_entidad.cod_empresa = lista[i].cod_empresa;
                o_entidad.cod_unidad = lista[i].cod_unidad;
                o_entidad.cod_veoregistro = s_codigo;
                o_entidad.fec_veoregistro = lista[i].fec_veoregistro.Substring(0, 19);
                o_entidad.cod_veoplantilla = lista[i].cod_veoplantilla;
                o_entidad.des_veoplantilla = lista[i].des_veoplantilla;
                o_entidad.cod_tipoubicacion = lista[i].cod_tipoubicacion;
                o_entidad.cod_sisgestion = lista[i].cod_sisgestion;
                o_entidad.cod_labor = lista[i].cod_labor;
                o_entidad.cod_lugar = lista[i].cod_lugar;
                o_entidad.cod_equipo = lista[i].cod_equipo;
                o_entidad.cum_veoregistro = lista[i].cum_veoregistro;
                o_entidad.noc_veoregistro = lista[i].noc_veoregistro;
                o_entidad.noa_veoregistro = lista[i].noa_veoregistro;
                o_entidad.por_veoregistro = lista[i].por_veoregistro;
                o_entidad.par_veoregistro = lista[i].par_veoregistro;
                o_entidad.usuario = VarGlobal.cod_usuario;
                o_entidad.ip = VarGlobal.ip;
                o_entidad.estado = "A";
                o_entidad.comando = "INS";

                //Lineas de Control
                List<lc_pro_veoregistro_lncontrol> lst_control = lc_pro_veoregistro_lncontrol_Data.Listar().Where(x => x.cod_veoregistro == s_codigo).ToList();
                o_entidad.lst_lc_pro_veoregistro_lncontrol = lst_control;

                //Participantes
                List<lc_pro_participante> lst_participante = lc_pro_participante_Data.Listar().Where(x => x.cod_referencia == s_codigo).ToList();
                o_entidad.lst_lc_pro_participante = lst_participante;

                //Coordenadas
                List<lc_pro_coordenada> lst_coordenada = lc_pro_coordenada_Data.Listar().Where(x => x.cod_referencia == s_codigo).ToList();
                o_entidad.lst_lc_pro_coordenada = lst_coordenada;

                if (s_codigo.Length != 12) { o_entidad.cod_veoregistro = ""; }

                var response = VarGlobal.LoadWebApiPut("pro_veoregistro", o_entidad);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "Operación no realizada...", "Aceptar");
                }
                else
                {
                    //Actualizando campo de sincronizado
                    lista[i].sincronizado = true;
                    lc_pro_veoregistro_Data.Modificar(lista[i]);
                }
                //Contador();
            }
        }
    }
}
