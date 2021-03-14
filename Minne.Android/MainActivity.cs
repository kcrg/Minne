using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Minne.Droid
{
    [Activity(MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTheme(Resource.Style.MainTheme);

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            LoadApplication(new App(new AndroidInitializer()));

            SetStatusBarColor();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();

            SetStatusBarColor();
        }

        public void SetStatusBarColor()
        {
            var currentTheme = Xamarin.Forms.Application.Current.RequestedTheme;

            var color = currentTheme == OSAppTheme.Light ? Android.Graphics.Color.ParseColor("#e0e0e0") : Android.Graphics.Color.ParseColor("#303030");

            var window = Platform.CurrentActivity.Window;

            if (window is null)
            {
                return;
            }

            window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color);
            window.SetNavigationBarColor(color);

            const Android.Views.StatusBarVisibility statusBarVisibility = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar
                | (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightNavigationBar;
            window.DecorView.SystemUiVisibility = currentTheme == OSAppTheme.Light ? statusBarVisibility : 0;
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}