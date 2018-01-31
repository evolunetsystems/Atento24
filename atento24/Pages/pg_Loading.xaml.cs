using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace atento24.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pg_Loading : PopupPage
    {
        public pg_Loading()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
