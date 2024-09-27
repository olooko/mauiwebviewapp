using MauiWebViewApp.ViewModels;

namespace MauiWebViewApp.Views;

public partial class SplashPage : ContentPage
{
	public SplashPage(SplashPageViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}
}