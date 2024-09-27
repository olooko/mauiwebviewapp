using MauiWebViewApp.ViewModels;

namespace MauiWebViewApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;
        }
    }
}
