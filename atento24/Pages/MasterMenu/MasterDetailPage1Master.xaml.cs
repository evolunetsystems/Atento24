using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using atento24.Data.DataLite;
using atento24.Data.Entidades;
using atento24.Data.ORM;
using atento24.Recursos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1Master : ContentPage
    {
        public ListView ListView;

        public MasterDetailPage1Master()
        {
            InitializeComponent();
            CargarMenu();
        }

        private void CargarMenu()
        {
            List<lc_acc_menu> lst_menu = new List<lc_acc_menu>();

            if (!string.IsNullOrEmpty(VarGlobal.cod_unidad))
            {
                lc_acc_menu_Data o_Data = new lc_acc_menu_Data();
                lst_menu = o_Data.Listar().Where(x => x.cod_empresa == VarGlobal.cod_empresa).ToList();
            }

            lst_menu.Add(new lc_acc_menu
            {
                cod_menu = "SINCRO",
                nom_menu = "Sincronizar",
                ico_menu = "fa-cloud"
            });

            lst_menu.Add(new lc_acc_menu
            {
                cod_menu = "UNIDAD",
                nom_menu = "Seleccionar Unidad",
                ico_menu = "fa-map-signs"
            });

            lst_menu.Add(new lc_acc_menu
            {
                cod_menu = "0000",
                nom_menu = "Cerrar Sesión",
                ico_menu = "fa-sign-out"
            });

            PermisoListView.ItemsSource = lst_menu;
            BindingContext = new MasterPageMasterViewModel();
            ListView = PermisoListView;
        }

        class MasterPageMasterViewModel : INotifyPropertyChanged
        {

            public ObservableCollection<acc_permiso> MenuItems { get; set; }


            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
