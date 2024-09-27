using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWebViewApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _webViewSource;

        public MainPageViewModel()
        {
            
        }

        [RelayCommand]
        private void Navigate()
        {
            this.WebViewSource = "https://dotnet.microsoft.com/apps/maui";
        }
    }
}
