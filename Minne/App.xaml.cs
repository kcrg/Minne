using Minne.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("MDL2.ttf", Alias = "FontIcons")]
[assembly: ExportFont("Lato-Bold.ttf", Alias = "LatoBold")]
[assembly: ExportFont("Lato-Semibold.ttf", Alias = "LatoSemiBold")]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Minne
{
    public partial class App
    {
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Routing.RegisterRoute("about", typeof(AboutPage));
            //Routing.RegisterRoute("about/settings", typeof(SettingsPage));

            Routing.RegisterRoute("todolist", typeof(ToDoListPage));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AppShell>();
        }
    }
}
