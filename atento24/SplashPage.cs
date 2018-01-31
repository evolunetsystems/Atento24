using atento24.Pages;
using Plugin.Iconize;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace atento24
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class SplashPage : ContentPage
    {
        public static string codigo { get; set; }
        Image splashImage;

        public SplashPage(string s_cod)
        {
            codigo = s_cod;
            NavigationPage.SetHasNavigationBar(this, false);
            var sub = new AbsoluteLayout();

            StackLayout stCon = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#0199DC"),
                Children = {
                    new IconImage
                    {
                        Icon = "fa-cloud",
                        IconColor = Color.FromHex("#E6E6E6"),
                        IconSize = 50,
                        WidthRequest = 70,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Margin = new Thickness(30, 40, 0, 15)
                    },
                    new IconImage
                    {
                        Icon = "fa-cloud",
                        IconColor = Color.FromHex("#D8D8D8"),
                        IconSize = 30,
                        WidthRequest = 50,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Margin = new Thickness(0, 20, 20, 15)
                    },
                    new IconImage
                    {
                        Icon = "fa-cloud",
                        IconColor = Color.FromHex("#D8D8D8"),
                        IconSize = 40,
                        WidthRequest = 70,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        Margin = new Thickness(60, 0, 0, 35)
                    },
                    new StackLayout
                    {
                        BackgroundColor = Color.Transparent,
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 100,
                        Children = {
                            new IconImage
                            {
                                Icon = "fa-cloud",
                                IconColor = Color.White,
                                IconSize = 90,
                                HorizontalOptions = LayoutOptions.Center,
                                WidthRequest = 100,
                            },
                            new IconImage
                            {
                                Icon = "fa-clock-o",
                                IconColor = Color.FromHex("#0199DC"),
                                IconSize = 22,
                                WidthRequest = 60,
                                Margin = new Thickness(-63, -21, 0, 0)
                            },
                            new Label
                            {
                                Text ="24",
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Color.FromHex("#0199DC"),
                                FontSize = 43,
                                Margin = new Thickness(-86, 30, 0, 0)
                            }
                        }
                    },
                    new Label
                    {
                        Text ="Atento24",
                        FontAttributes = FontAttributes.Italic,
                        TextColor = Color.White,
                        FontSize = 25,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(0, -20, 0, 0)
                    },
                    new ActivityIndicator
                    {
                        IsRunning = true,
                        Margin = new Thickness(0, 20, 0, 0),
                        Color = Color.White,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End
                    },
                    new Label
                    {
                        Text ="Por favor espere. Autorizando...",
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End
                    }
                }
            };

            splashImage = new Image
            {
                Source = "",
                HeightRequest = 150,
                WidthRequest = 250
            };

            BackgroundColor = Color.FromHex("#BDBDBD");
            Content = stCon;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splashImage.ScaleTo(1, 2000); //Time-consuming processes such as initialization

            switch (codigo)
            {
                case "sin_login":
                    Application.Current.MainPage = new MainPage("ok");
                    //Application.Current.MainPage = new pg_sincronizar();
                    break;
                case "con_login":
                    Application.Current.MainPage = new MasterDetailPage1("pg_empresa");
                    break;
                case "":
                    break;
            }
        }
    }
}
