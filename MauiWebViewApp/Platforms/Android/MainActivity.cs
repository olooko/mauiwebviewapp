using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace MauiWebViewApp
{
    [Activity(Theme = "@style/Maui.NoSplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            DeviceDisplay.Current.KeepScreenOn = true;

            if (Window != null)
            {
                SetWindow(Window);
            }
        }

        static void SetWindow(Android.Views.Window window)
        {
#pragma warning disable CA1416
            IWindowInsetsController? wicController = window.InsetsController;

            window.SetDecorFitsSystemWindows(false);
            window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            if (wicController != null)
            {
                wicController.Hide(WindowInsets.Type.Ime());
                wicController.Hide(WindowInsets.Type.NavigationBars());
            }
#pragma warning restore CA1416
        }
    }
}
