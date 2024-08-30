using MauiWebViewApp.ViewModels;

namespace MauiWebViewApp.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingPageViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}
}