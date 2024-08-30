using CommunityToolkit.Maui;
using MauiWebViewApp.Views;
using MauiWebViewApp.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using System.Runtime.InteropServices;
using Windows.Graphics;
#endif 

namespace MauiWebViewApp
{
    public static class MauiProgram
    {
#if WINDOWS
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);
#endif

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        try
                        {
                            window.ExtendsContentIntoTitleBar = false;

                            IntPtr handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            WindowId windowId = Win32Interop.GetWindowIdFromWindow(handle);
                            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

                            if (appWindow.Presenter is OverlappedPresenter p)
                            {
                                p.IsResizable = false;
                                p.SetBorderAndTitleBar(false, false);
                            }

                            int x = 0;
                            int y = 0;
                            int width = (int)DeviceDisplay.Current.MainDisplayInfo.Width;
                            int height = (int)DeviceDisplay.Current.MainDisplayInfo.Height;

                            appWindow.MoveAndResize(new RectInt32(x, y, width, height));

                            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
                            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DONOTROUND;

                            DwmSetWindowAttribute(handle, attribute, ref preference, sizeof(uint));
                        }
                        catch (Exception)
                        {

                        }
                    });
                });
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransientWithShellRoute<LoadingPage, LoadingPageViewModel>(nameof(LoadingPage));
            builder.Services.AddTransientWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPage));

            return builder.Build();
        }
    }
}
